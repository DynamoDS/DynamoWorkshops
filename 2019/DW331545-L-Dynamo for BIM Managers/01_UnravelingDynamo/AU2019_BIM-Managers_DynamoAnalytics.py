"""
Dynamo graph check and usage tracking.
"""

__author__ = 'Dynamo BIM Managers: AU 2019'
__version__ = '1.0'


import clr
import sys
sys.path.append(r'C:\Program Files (x86)\IronPython 2.7\Lib')
import System
from System import Environment
from System.IO import *
import os
import csv

"""
# Old way (Revit 2019 and down) to access the Dynamo DLLs
dynamoAPIFolder = Directory.CreateDirectory(r'C:\Program Files\Dynamo\Dynamo Core')
latest = sorted([s.Name for s in dynamoAPIFolder.GetDirectories()])[-1]
clr.AddReferenceToFileAndPath(Path.Combine(dynamoAPIFolder.FullName, latest, 'DynamoCore.dll'))
clr.AddReferenceToFileAndPath(Path.Combine(dynamoAPIFolder.FullName, latest, 'DynamoCoreWpf.dll'))
"""

# New way to access Dynamo DLLs as Revit now incorporates Dynamo as a feature
sys.path.append(r'C:\Program Files\Autodesk\Revit 2020\AddIns\DynamoForRevit')
sys.path.append(r'C:\Program Files\Autodesk\Revit 2020\AddIns\DynamoForRevit\Revit')
    
clr.AddReference('DynamoCore.dll')
clr.AddReference('DynamoCoreWpf.dll')
clr.AddReference('DynamoRevitDS.dll')

from Dynamo.ViewModels import *
from Dynamo.Models import *
from Dynamo.Graph.Workspaces import *

try:
    
    clr.AddReference("RevitServices")
    import RevitServices
    from RevitServices.Persistence import DocumentManager
    
    doc = DocumentManager.Instance.CurrentDBDocument
    uiapp = DocumentManager.Instance.CurrentUIApplication
    revitApp = uiapp.Application
    
    clr.AddReference("RevitAPI")
    import Autodesk
    from Autodesk.Revit.DB import *
    
    if doc.IsWorkshared:
        revitDoc = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath())
    else:
        revitDoc = DocumentManager.Instance.CurrentDBDocument.PathName
    
    if len(revitDoc) == 0:
        revitDoc = 'None'
    
    version = doc.Application.VersionNumber
    
    from Dynamo.Applications import *
    app = DynamoRevit()
    model = app.RevitDynamoModel
    ws = model.CurrentWorkspace
    dynpath = ws.FileName # Gets the Dynamo File Path
    dynname = Path.GetFileName(dynpath) # Gets the Dynamo File Name
    
    nodeCount = len(ws.Nodes)  # Counts the total amount of Nodes in the Workspace
    wireCount = len(list(ws.Connectors))  # Counts the total amount of Wires in the Workspace
    lastSave = ws.LastSaved  # Get's the time and date of last Graph Save
    notes = list(ws.Notes)  # Captures the Workspace Note objects and casts to a list
    
    notesList,catchList = [],[]
    for line in notes:
        catchList.append(line.Text) # Get's the Text content of each Note

    keys = ["OFFICE: ", "AUTHOR NAME: ", "AUTHOR EMAIL: ", "TESTED ON: ", "RATING: ", "KEYWORDS: "] # Template data to export

    for k in keys:  # Captures only the relevant Note Data based around a colon split
        for n in catchList:
            if k in n:
                notesList.append(n.split(':')[-1])
            else:
                pass
                
    dataOffice = notesList[0]
    dataAuthorName = notesList[1]
    dataAuthorEmail = notesList[2]
    dataTestedOn = notesList[3]
    dataRating = notesList[4]
    dataKeywords = notesList[5]
    
    appName = revitApp.VersionName # Gets the Revit Version 
    appNumber = revitApp.VersionNumber # Gets the Revit Version Number
    appBuild = revitApp.VersionBuild # Gets the Revit Version Build
    appLanguage = str(revitApp.Language) # Gets the Revit Language
    
    # Getting Physical Memory (RAM) of Machine
    process = os.popen('wmic memorychip get capacity')
    result = process.read()
    process.close()
    totalMem = 0
    
    for m in result.split(" \r\n")[1:-1]:
        totalMem += int(m)
    
    machineRam = totalMem / (1024**3) # Gets the Machines Ram
    
    # Machine data
    userName = Environment.UserName # Gets the Username
    machineName = Environment.MachineName # Gets the Machines Name
    machineOS = Environment.OSVersion # Gets the OS Version
    machineProcessor = Environment.ProcessorCount # Gets the Machines Processor Count
    machineVersion = Environment.Version # Gets the Machines Version
    

    summary = {
                "Machine" :
                    {
                    "Username" : userName,
                    "Name" : machineName,
                    "OS" : machineOS,
                    "Processor Count" : machineProcessor,
                    "Version" : machineVersion,
                    "RAM" : machineRam
                    },
                "Dynamo" :
                    {
                    "Path" : dynpath,
                    "Name" : dynname,
                    "Node Count" : nodeCount,
                    "Wire Count" : wireCount,
                    "Last Saved" : lastSave
                    },
                "Revit" :
                    {
                    "Version Name" : appName,
                    "Version Number" : appNumber,
                    "Version Build" : appBuild,
                    "Language" : appLanguage,
                    "Revit Document" : revitDoc
                    },
                "Graph Notes" :
                    {
                    "Office" : dataOffice,
                    "Author Name" : dataAuthorName,
                    "Author Email" : dataAuthorEmail,
                    "Last Tested On" : dataTestedOn,
                    "Rating" : dataRating,
                    "Keywords" : dataKeywords
                    }
                }

except Exception as ex:
    summary = ex.message

###############################################################################################################################
###########             CHANGE THE FILE PATH AND FILE NAME TO THE SHARED LOCATION               ###############################
###############################################################################################################################
tracker = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 'DynamoTracker_' + userName + '.csv')

trace = [userName, machineName, machineOS, machineProcessor, machineRam,
         machineVersion, appName, appNumber, appBuild, appLanguage, revitDoc, 
         dynname, dynpath, lastSave, nodeCount, wireCount, dataOffice, 
         dataAuthorName, dataAuthorEmail, dataTestedOn, dataRating, dataKeywords]

headers = ['User: Username', 'Machine: Name', 'Machine: Operating System', 'Machine: Processor', 
            'Machine: RAM', 'Machine: Version','Application: Name', 'Application: Version', 
            'Application: Build Number', 'Application: Language', 'Application: Document Path', 
            'Graph: Name', 'Graph: Path', 'Graph: Last Saved', 'Graph: Node Count', 
            'Graph: Wire Count', 'Info: Office', 'Info: Author Name', 'Info: Author Email', 
            'Info: Tested On', 'Info: Rating', 'Info: Keywords']

if os.path.isfile(tracker):
    with open(tracker, 'ab') as f:
        writer = csv.writer(f, dialect='excel')
        writer.writerow(trace)
else:
    with open(tracker, 'ab') as f:
        writer = csv.writer(f, dialect='excel')
        writer.writerow(headers)
        writer.writerow(trace)
trace = []