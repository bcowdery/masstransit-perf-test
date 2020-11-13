Service Bus Performance Testing
==============================================

Simple performance testing for messaging transport layers.


**Requirements:**
* [Docker](https://www.docker.com/products/docker-desktop)
* [Taskfile](https://taskfile.dev)

&nbsp;

# Getting Started

You can build, package and run the entire solution with `up`.

```shell
$ task up
```

&nbsp;

## Tasks

Available tasks for this project:

| Task      | Description                                             |
|-----------|---------------------------------------------------------|
| `up`      | Bring up the solution                                   |
| `down`    | Bring down the solution                                 |
| `restart` | Restart the solution                                    |
| `status`  | Display the status of all containers in the solution    |
| `logs`    | Display logs from all containers in the solution        |
| `build`   | Builds all container images                             |
| `pull`    | Pulls the most recent container images                  |
| `push`    | Pushes container images to remote repositories          |
| `prune`   | Prune obsolete remote git branches and local containers |
| `nuke`    | Nuke all data files and reset the solution              |


&nbsp;

# Services

| Service          | Role                                 | Local Port | Private Port | URL                     |
|------------------|--------------------------------------|------------|--------------|-------------------------|
| SQL Server       | Database server                      | 11433      | 1433         |                         |
| RabbitMQ         | Message broker for pub/sub messaging | 15672      | 5672         | https://localhost:15672 |


&nbsp;

> ✨ Port numbers, usernames and passwords are all defined in the environment `.env` file.

