#!/bin/bash

set -e
run_cmd="dotnet OzonEdu.MerchandiseService.dll --no-build -v d"

dotnet OzonEdu.MerchandiseService.Migrator.dll --no-build -v d -- --dry-run

dotnet OzonEdu.MerchandiseService.Migrator.dll --no-build -v d

>&1 echo "MerchandiseService DB Migrations complete, starting app."
>&1 echo "Run MerchandiseService: $run_cmd"
exec $run_cmd