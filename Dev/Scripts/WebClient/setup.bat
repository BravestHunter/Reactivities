@echo off

docker volume create ^
  --driver local ^
  --opt type=none ^
  --opt o=bind ^
  --opt device=%~dp0\..\..\..\Reactivities.WebClient\node_modules ^
  reactivities-nodemodules