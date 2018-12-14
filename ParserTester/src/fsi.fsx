// load from src/

// packages
#r @"../packages/Argu/lib/netstandard2.0/Argu.dll"
#r @"../packages/FsCheck/lib/netstandard2.0/FsCheck.dll"
#r @"../packages/Expecto/lib/netstandard2.0/Expecto.dll"
// external
#r @"../../Parser/bin/Debug/netcoreapp2.1/QUT.ShiftReduceParser.dll"
#r @"../../Parser/bin/Debug/netcoreapp2.1/Parser.dll"
#load "External.fs"
// lib
#r @"../../PPrint/bin/Release/netstandard2.0/PPrint.dll"
#r @"../../Language//bin/Release/netstandard2.0/Language.dll"
#load "Extensions.fs"
#load "GenExtensions.fs"
#load "RunProcess.fs"
// generator
#load "Generator/PrettyPrintBadly.fs"
#load "Generator/Generate.fs"
// application
#load "Error.fs"
#load "Application.fs"
#load "Program.fs"
