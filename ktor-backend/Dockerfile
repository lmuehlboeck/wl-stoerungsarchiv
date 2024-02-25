FROM gradle:7-jdk11 AS build
COPY --chown=gradle:gradle . /home/gradle/src
WORKDIR /home/gradle/src
RUN gradle buildFatJar --no-daemon

FROM openjdk:11
ENV TZ=Europe/Vienna
RUN ln -sf /usr/share/zoneinfo/Europe/Vienna /etc/localtime
RUN mkdir /app
COPY --from=build /home/gradle/src/build/libs/*.jar /app/ktor-backend.jar
ENTRYPOINT ["java","-jar","/app/ktor-backend.jar"]