#!/bin/sh

SOURCE_PATH="~/test" # replace
DEST_PATH="." # replace
FILENAME="backup_$(date +"%d-%m-%Y").tar.gz"
SET_CRONJOB=true
FULL_DEST_PATH="$DEST_PATH/$FILENAME"
CRON_TIMESPEC="0 12 * * *" # replace
SCRIPT_NAME=$(basename -- "$0")
SCRIPT_PATH=$(dirname -- "$0")
SUBMIT_URL="localhost" # replace

if [ -n "$1" ]
then
    case "$1" in
        -c) SET_CRONJOB=true ;;
        *) SET_CRONJOB=false ;;
    esac
fi

tar -zcvf "$FULL_DEST_PATH" "$SOURCE_PATH"

curl -i -X POST -H "Content-Type: multipart/form-data" -F "data=@$FULLDEST_PATH" $SUBMIT_URL

if [ $SET_CRONJOB ]
then
    (crontab -l 2>/dev/null; echo "$CRON_TIMESPEC sh '$SCRIPT_PATH/$SCRIPT_NAME' ") | crontab -
fi