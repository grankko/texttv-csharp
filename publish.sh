#!/bin/bash

set -e

dotnet publish src/TextTv.Cli/TextTv.Cli.csproj -c Release -r linux-x64 -p:PublishSingleFile=true -p:AssemblyName=texttv --self-contained -o publish