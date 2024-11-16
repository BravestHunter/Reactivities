@echo off

docker volume create ^
  --driver local ^
  --opt type=none ^
  --opt o=bind ^
  --opt device=%~dp0\..\..\node_modules ^
  reactivities-nodemodules