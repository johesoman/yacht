// load from src/

// packages
#r @"../packages/Argu/lib/netstandard2.0/Argu.dll"
#r @"../packages/FsCheck/lib/netstandard2.0/FsCheck.dll"
#r @"../packages/Expecto/lib/netstandard2.0/Expecto.dll"
// external
#r @"../Parser/bin/Release/netstandard2.0/QUT.ShiftReduceParser.dll"
#r @"../Parser/bin/Release/netstandard2.0/Parser.dll"
// lib
#r @"../Extensions/bin/Release/netstandard2.0/Extensions.dll"
#r @"../PPrint/bin/Release/netstandard2.0/PPrint.dll"
#r @"../Language/bin/Release/netstandard2.0/Language.dll"
#r @"../ParserTester/bin/Release/netstandard2.0/ParserTester.dll"

#load "External.fs"
#load "RunProcess.fs"
// application
#load "Error.fs"
#load "Application.fs"
#load "Program.fs"
