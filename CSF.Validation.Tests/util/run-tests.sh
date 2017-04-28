#!/bin/sh

SOLUTION_ROOT="$(dirname "$0")/../.."
RUNNER_PATH="packages/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe"
TEST_ASSEMBLIES="CSF.Validation.Tests/bin/Debug/CSF.Validation.Tests.dll"

mono "${SOLUTION_ROOT}/${RUNNER_PATH}" $TEST_ASSEMBLIES