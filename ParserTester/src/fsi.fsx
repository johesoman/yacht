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
#load "Extensions.fs"
#load "GenExtensions.fs"
#load "PPrint.fs"
#load "RunProcess.fs"
// language
#load "Language/LTypes.fs"
#load "Language/LPrettyPrint.fs"
// generator
#load "Generator/PrettyPrintBadly.fs"
#load "Generator/Generate.fs"
// application
#load "Error.fs"
#load "Application.fs"
#load "Program.fs"
