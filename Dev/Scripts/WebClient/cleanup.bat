docker volume rm reactivities-nodemodules

set NODE_MODULES_PATH=%~dp0\..\..\node_modules

if exist "%NODE_MODULES_PATH%" (
    echo Deleting directory: %NODE_MODULES_PATH%
    rmdir /s /q "%NODE_MODULES_PATH%"
)