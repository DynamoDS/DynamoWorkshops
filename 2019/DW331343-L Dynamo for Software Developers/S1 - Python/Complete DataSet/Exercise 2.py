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

import math

doc = DocumentManager.Instance.CurrentDBDocument
uiapp = DocumentManager.Instance.CurrentUIApplication
app = uiapp.Application

#Uncomment the line below to enable the first input
desks = IN[0]
room = IN[1]

# convert to m
deskRadius = IN[2] / 1000.0

# Get the area of the desk in m2
deskArea = math.pi * (deskRadius * deskRadius)
totalDesks = len(desks)

roomArea = room.Area

ratio = roomArea / (totalDesks * deskArea)

OUT = ratio
