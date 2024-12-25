# Webstore API

Webstore API is a fully-featured e-commerce backend solution built with **ASP.NET Core**. It provides secure authentication, product management, order processing, payment integration, and a comprehensive set of features necessary for an e-commerce platform. The system supports advanced functionalities such as email verification, password reset, and role-based access (Admin & Customer).

## Features

- **Secure Authentication**: JWT-based authentication and role management (Admin & Customer).
- **Product Management**: Full CRUD functionality for managing products, categories, and brands.
- **Shopping Basket**: Manage the user’s shopping cart, add/remove products.
- **Order Management**: Place, view, and manage orders with delivery methods.
- **Payment Integration**: Secure payment processing via **Stripe**.
- **Email Verification**: OTP-based email verification for user security.
- **Password Reset**: Forget and reset password functionality with OTP.
- **Search, Sorting, Pagination, & Filtering**: Efficient product discovery.
- **Caching with Redis**: Optimized performance with Redis for frequently accessed data.
- **Role-based Authorization**: Separate roles for Admin and Customer to manage access.

## Technologies Used

- **ASP.NET Core**: Framework for building the API.
- **JWT Authentication**: For secure user authentication.
- **Stripe**: Payment gateway integration.
- **Redis**: Caching mechanism for enhanced performance.
- **Entity Framework Core**: ORM for database management.
- **Onion Architecture**: Structured design with separation of concerns.
- **Unit of Work & Generic Repository Patterns**: Clean and scalable data access.
- **Specification Pattern**: For building dynamic queries.

## API Endpoints

### Accounts

- **POST /api/Accounts/register**: Register a new user.
- **POST /api/Accounts/EmailVerification**: Verify email with OTP.
- **POST /api/Accounts/login**: User login, returns a JWT token.
- **POST /api/Accounts/ForgetPassword**: Request password reset link.
- **POST /api/Accounts/ResetPassword**: Reset password with OTP.
- **GET /api/Accounts/GetCurrentUser**: Get details of the currently logged-in user.
- **GET /api/Accounts/Address**: Fetch user’s saved addresses.

### Basket

- **GET /api/Basket**: Get the user’s basket details.
- **POST /api/Basket**: Add a product to the basket.
- **DELETE /api/Basket**: Remove a product from the basket.

### Brands

- **GET /api/Brands**: Get a list of all brands.
- **POST /api/Brands**: Add a new brand.
- **GET /api/Brands/{id}**: Get a specific brand by ID.
- **PUT /api/Brands/{id}**: Update an existing brand.
- **DELETE /api/Brands/{id}**: Delete a brand.

### Orders

- **POST /api/Orders**: Place a new order.
- **GET /api/Orders/GetOrdersForSpecificUser**: Get all orders for the current user.
- **GET /api/Orders/GetOrderForSpecificUser/{orderId}**: Get details of a specific order.
- **GET /api/Orders/GetDeliveryMethods**: Get available delivery methods.
- **DELETE /api/Orders/{orderId}**: Cancel an order.
- **GET /api/Orders/all**: Get all orders (Admin only).

### Payment

- **POST /api/Payment/{basketId}**: Process payment for the basket items.
- **POST /api/Payment/Webhook**: Webhook to handle payment updates from Stripe.

### Products

- **GET /api/Products**: Get a list of all products.
- **POST /api/Products**: Add a new product.
- **GET /api/Products/{id}**: Get a product by ID.
- **PUT /api/Products/{id}**: Update an existing product.
- **DELETE /api/Products/{id}**: Delete a product.

### Types

- **GET /api/Types**: Get a list of all product types.
- **POST /api/Types**: Add a new product type.
- **GET /api/Types/{id}**: Get a product type by ID.
- **PUT /api/Types/{id}**: Update an existing product type.
- **DELETE /api/Types/{id}**: Delete a product type.

---

## Architecture

The Webstore API is built using **Onion Architecture**, which emphasizes separation of concerns and ensures clean, maintainable code. The core layers include:

- **Core Layer**: Contains business logic and domain models.
- **Application Layer**: Implements service interfaces and application logic.
- **Infrastructure Layer**: Deals with external dependencies (e.g., database access, Stripe integration).
- **API Layer**: The entry point for all client interactions through HTTP requests.

### Design Patterns

- **Unit of Work**: Manages transaction boundaries and ensures data consistency.
- **Generic Repository**: Provides a standard interface for data access and minimizes redundant code.
- **Specification Pattern**: Allows dynamic query creation for complex searches and filters.
- **AutoMapper**: Simplifies object-to-object mapping, especially for data transfer objects (DTOs).

---

## Caching

Redis is used for caching commonly accessed data like products, improving response times and reducing database load. This is especially useful for high-traffic e-commerce platforms.

---

## Error Handling

Comprehensive error handling is implemented using global exception handling middleware, providing meaningful error messages and ensuring a smooth user experience.

---

## Stripe Integration

Payments are processed securely via the **Stripe API**, allowing users to make purchases using credit/debit cards. Webhooks are set up to handle payment updates and status changes.

---

## Deployment

This API is designed for easy deployment on cloud platforms and includes all necessary configurations for production environments. It is hosted on **MonsterAPI** for high availability and scalability.

---

