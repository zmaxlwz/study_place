@echo off
title My Test Batch file
:: this is a comment, not appear in output
echo Test file executed
echo I am too lazy to write commands again and again
set myValue=320
echo %myValue%
ping -n 10 127.0.0.1
pause
for /l %%x in (1, 1, 100000) do (
    echo %%x
    DIR /B | sort
)
pause
