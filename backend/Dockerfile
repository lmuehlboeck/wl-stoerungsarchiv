# pull official base image
FROM python:3.10.8-slim-buster

# set work directory
WORKDIR /app

# set environment variables
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

# set timezone
ENV TZ=Europe/Vienna
RUN ln -sf /usr/share/zoneinfo/Europe/Vienna /etc/localtime

# install dependencies
RUN pip install --upgrade pip
COPY ./requirements.txt /app/requirements.txt
RUN pip install -r requirements.txt

# copy project
COPY . /app

# install cron
RUN apt-get update
RUN apt-get -y install cron

# setup cron job
RUN echo "*/2 * * * * root cd /app && python3 update_db.py" >> /etc/crontab