#!/bin/bash

echo ******************************************************
echo Starting the replica set
echo ******************************************************

sleep 10 | echo Sleeping
mongo mongodb://mongo-1:3000 replica-set.js