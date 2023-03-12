@echo off
:loop
git pull
if ERRORLEVEL 1 (
    echo git pull failed,after 5 second retry
    timeout /t 5
    goto loop
) else (
    echo git pull successed
)