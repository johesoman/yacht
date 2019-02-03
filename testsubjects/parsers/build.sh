#!/usr/bin/env fish

cd $argv
msbuild /p:configuration=Release /verbosity:quiet
cd ..
