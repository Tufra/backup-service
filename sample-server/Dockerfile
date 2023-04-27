FROM python:alpine

ADD ./share /usr/src/share
COPY ./server.py /usr/src

WORKDIR /usr/src
USER guest
ENV PYTHONUNBUFFERED=1

EXPOSE 8000

ENTRYPOINT [ "python3", "server.py" ]
