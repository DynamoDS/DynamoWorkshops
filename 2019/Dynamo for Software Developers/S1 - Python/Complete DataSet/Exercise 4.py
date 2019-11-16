#Copyright 2019. All rights reserved. Bimorph Consultancy LTD, 5 St Johns Lane, London EC1M 4BH www.bimorph.com
#Written by Thomas Mahon @Thomas__Mahon info@bimorph.com Package: BimorphNodes
#Follow: facebook.com/bimorphBIM | linkedin.com/company/bimorph-bim | @bimorphBIM

import clr
clr.AddReference("ProtoGeometry")
from Autodesk.DesignScript.Geometry import *

# Import ToDSType(bool) extension method
clr.AddReference("RevitNodes")
import Revit
clr.ImportExtensions(Revit.Elements)

# Import geometry conversion extension methods
clr.ImportExtensions(Revit.GeometryConversion)

# Import DocumentManager and TransactionManager
clr.AddReference("RevitServices")
import RevitServices
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager
from System.Collections.Generic import *

# Import RevitAPI
clr.AddReference("RevitAPI")
import Autodesk
from Autodesk.Revit.DB import *

doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application

#Uncomment the line below to enable the first input
wallLocationTopCurve = IN[0]
wallLocationBottomCurve = IN[1]
totalParitions = int(IN[2])
wallType = UnwrapElement(IN[3])
level = UnwrapElement(IN[4])

topCurve = wallLocationTopCurve.ToRevitType()
bottomCurve = wallLocationBottomCurve.ToRevitType()

curveLength = topCurve.Length
subdividedLength = curveLength / (totalParitions + 1)

# "Start" the transaction
TransactionManager.Instance.EnsureInTransaction(doc)

wallList = []
for i in range(totalParitions):
    pointAlongCurve = topCurve.Evaluate((i + 1) * subdividedLength, False)
    projectToBottomResult = bottomCurve.Project(pointAlongCurve)

    newLocationCurve = Line.CreateBound(pointAlongCurve, projectToBottomResult.XYZPoint)
    newWall = Wall.Create(doc, newLocationCurve, wallType.Id, level.Id, 6.0, 0.0, False, False)

    wallList.append(newWall.ToDSType(False))

# "End" the transaction
TransactionManager.Instance.TransactionTaskDone()

#Uncomment the line below to output an object
OUT = wallList