# Ecommerce Microservices

This project is a microservices-based e-commerce solution built with .NET Core, Angular, and various databases. It demonstrates a scalable architecture for online shopping platforms.

## Purpose

This project serves as a learning platform for developing, experimenting, solving problems, and seeking help online. Through building this e-commerce microservices application, I have learned and implemented:

- **Microservice Architecture**: Comparison with monolithic architecture and its advantages/drawbacks
- **Clean Architecture**: Applied in the Users microservice
- **Data Management & Synchronization**: Using event-driven architecture with message brokers (RabbitMQ)
- **Interservice Communication**: Synchronous and asynchronous communication patterns in microservices
- **API Gateway**: Routing and managing requests using Ocelot
- **Caching**: Implementation using Redis
- **Containerization**: Using Docker and Docker Compose
- **Orchestration**: Kubernetes deployment on Azure Kubernetes Service (AKS)
- **Azure API Management (APIM)**: API management and security
- **Azure DevOps and CI/CD**: Continuous integration and deployment pipelines
- **Fault Tolerance**: Using Polly for resilient API calls
- And more...

## Architecture

The application consists of the following microservices:

- **API Gateway**: Routes requests to appropriate microservices using Ocelot
- **Orders Microservice**: Handles order management using MongoDB
- **Products Microservice**: Manages product catalog using MySQL
- **Users Microservice**: Manages user accounts and authentication using PostgreSQL
- **Frontend**: Angular application for the user interface

## Technologies Used

- **Backend**: .NET Core, ASP.NET Web API
- **Frontend**: Angular
- **Databases**: MongoDB, MySQL, PostgreSQL
- **Messaging**: RabbitMQ
- **Caching**: Redis
- **Containerization**: Docker
- **Orchestration**: Kubernetes
- **API Gateway**: Ocelot

## Prerequisites

- Docker and Docker Compose
- .NET Core SDK (for local development)
- Node.js and Angular CLI (for frontend development)
- kubectl (for Kubernetes deployment)

## Getting Started

### Using Docker Compose

1. Clone the repository
2. Navigate to the root directory
3. Run the following command to build and start all services:

```bash
docker-compose -f docker-compose.build.yaml up --build
```

This will start all microservices, databases, and infrastructure components.

### Accessing the Application

- **API Gateway**: http://localhost:5000
- **Frontend**: http://localhost:4200 (after starting the Angular app separately)
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

### Individual Service Development

Each microservice can be run independently for development:

#### Orders Microservice

```bash
cd eCommerceSolution.OrdersService
docker-compose up --build
```

#### Products Microservice

```bash
cd eCommerceSolution.ProductsService
# Build and run commands
```

#### Users Microservice

```bash
cd eCommerceSolution.UsersService
# Build and run commands
```

#### Frontend

```bash
cd frontend
npm install
ng serve
```

## Kubernetes Deployment

Kubernetes manifests are provided in the `aks/` directory for production deployment on Azure Kubernetes Service (AKS).

To deploy:

```bash
kubectl apply -f aks/
```

Individual service manifests are also available in their respective `k8s/` folders.

## CI/CD with Azure DevOps

This repository includes Azure DevOps CI/CD pipelines ready for integration and deployment to an AKS cluster in Azure. Each microservice contains an `azure-pipelines.yml` file configured for:

- Automated builds and testing
- Docker image creation and push to Azure Container Registry
- Deployment to AKS cluster

To use the pipelines:

1. Import the repository into Azure DevOps
2. Configure your Azure subscription and AKS cluster in Azure DevOps
3. Set up service connections for Azure Container Registry and AKS
4. Run the pipelines for each service or set up multi-stage pipelines

For complete step-by-step Azure setup instructions including Azure DevOps configuration, API Management, Azure AD B2C, and detailed pipeline configurations, refer to [`Complete Steps.yaml`](Complete%20Steps.yaml).

## Project Structure

```
├── aks/                          # Kubernetes manifests for AKS
├── Complete Steps.yaml           # Complete Azure setup and deployment guide
├── eCommerceSolution.OrdersService/    # Orders microservice
├── eCommerceSolution.ProductsService/  # Products microservice
├── eCommerceSolution.UsersService/     # Users microservice
├── frontend/                     # Angular frontend
├── mongodb/                      # MongoDB setup
├── mysql/                        # MySQL setup
├── postgres/                     # PostgreSQL setup
├── PostmanCollections/           # Postman collections for API testing
└── docker-compose.build.yaml     # Docker Compose for all services
```

## API Documentation

API endpoints are available through the API Gateway. Refer to individual service documentation for detailed API specs.

Postman collections for testing the APIs are available in the `PostmanCollections/` folder:

- `OrdersMicroservice.API.postman_collection.json`
- `ProductsMicroService.API.postman_collection.json`
- `Users Microservice.postman_collection.json`
