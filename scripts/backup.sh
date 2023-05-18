#!/bin/sh

BACKUP_NAME="{0}" # "test" replace
SOURCE_PATH="{1}" # "~/test" replace
DEST_PATH="{2}" # "." replace
FILENAME="${BACKUP_NAME}_backup_$(date +"%d-%m-%Y").tar.gz"
SET_CRONJOB=0 # true replace
FULL_DEST_PATH="$DEST_PATH/$FILENAME"
CRON_TIMESPEC="{3}" # "0 12 * * *" replace
SCRIPT_NAME=$(basename -- "$0")
SCRIPT_PATH=$(dirname -- "$0")
SUBMIT_URL="{4}" # localhost replace
TRANSFER_FILE={5} # 0 replace
KEEP_FILE={6} # 0 replace
LOG_FILE="/var/log/backup_log"


if [ -n "$1" ]
then
    case "$1" in
        -c) SET_CRONJOB=0 ;;
        *) SET_CRONJOB=1 ;;
    esac
fi

tar -zcvf "$FULL_DEST_PATH" "$SOURCE_PATH" 1>/dev/null
res=$?

if [ $res -eq 0 ]
then
    echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: backup created: $FULL_DEST_PATH" >> "$LOG_FILE"
else
    echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: backup failed" >> "$LOG_FILE"
fi

if [ $TRANSFER_FILE ]
then
    curl -i -X POST -H "Content-Type: multipart/form-data" -F "data=@$FULL_DEST_PATH" $SUBMIT_URL
    res=$?

    if [ $res -eq 0 ]
    then
	echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: file send success" >> "$LOG_FILE"
    else
	echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: file send failed" >> "$LOG_FILE"
    fi

    if [ $KEEP_FILE -eq 1 ]
    then
        rm "$FULL_DEST_PATH"
	echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: backup file removed" >> "$LOG_FILE"
    fi
fi

if [ $SET_CRONJOB -eq 0 ]
then
    (crontab -l 2>/dev/null; echo "$CRON_TIMESPEC sh '$SCRIPT_PATH/$SCRIPT_NAME' -c ") | crontab -
    echo "$(date +'[ %Y-%m-%d %H:%M ]') $BACKUP_NAME: cron job set at $CRON_TIMESPEC" >> "$LOG_FILE"
fi
