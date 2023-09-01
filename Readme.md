# Banking System run locally Instructions 

 When you run `docker-compose up --build` the docker compose file is located in the root of the banking system , 
 it will build and run the Docker container based on the specified Dockerfile,  and it will map port 5000 on your host machine to port 80 in the container.  This will allow you to access to the Web API at http://localhost:5000/swagger/index.html on your local machine.