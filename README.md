# 🛍️ Webstore API

**Webstore API** is a fully-featured e-commerce backend built with **ASP.NET Core**.  
It provides secure authentication, product & order management, payment integration, and all the essential features required for a modern online store.  

It also supports advanced functionalities like email verification, password reset, caching, and role-based access control.

---

## 🚀 Features

- 🔐 **Secure Authentication** – JWT-based auth with role management (Admin & Customer)  
- 🛒 **Product & Category Management** – Full CRUD for products, categories, and brands  
- 🧺 **Shopping Basket** – Add/remove items and manage user cart  
- 📦 **Order Management** – Place, view, and manage orders with delivery methods  
- 💳 **Payment Integration** – Secure online payments with **Stripe**  
- 📧 **Email Verification & OTP** – Enhanced user account security  
- 🔑 **Password Reset** – Forget/reset password with OTP verification  
- 🔍 **Search, Sorting, Filtering, Pagination** – Improved product discovery  
- ⚡ **Caching with Redis** – Faster performance for frequently accessed data  
- 🔐 **Role-based Authorization** – Separate roles for Admin and Customer  

---

## 🛠️ Technologies Used

- **ASP.NET Core** – API framework  
- **JWT** – Authentication & authorization  
- **Stripe** – Payment processing  
- **Redis** – Caching layer  
- **Entity Framework Core** – ORM for database  
- **Onion Architecture** – Clean project structure  
- **Unit of Work & Generic Repository** – Data access patterns  
- **Specification Pattern** – Dynamic query building  

---

## 📡 API Endpoints

### 🔑 Accounts  
- `POST /api/Accounts/register` – Register a new user  
- `POST /api/Accounts/EmailVerification` – Verify email with OTP  
- `POST /api/Accounts/login` – User login (returns JWT)  
- `POST /api/Accounts/ForgetPassword` – Request password reset  
- `POST /api/Accounts/ResetPassword` – Reset password with OTP  
- `GET /api/Accounts/GetCurrentUser` – Get current user info  
- `GET /api/Accounts/Address` – Get user addresses  

### 🧺 Basket  
- `GET /api/Basket` – Get basket details  
- `POST /api/Basket` – Add product to basket  
- `DELETE /api/Basket` – Remove product from basket  

### 🏷️ Brands  
- `GET /api/Brands` – Get all brands  
- `POST /api/Brands` – Add a new brand  
- `GET /api/Brands/{id}` – Get brand by ID  
- `PUT /api/Brands/{id}` – Update brand  
- `DELETE /api/Brands/{id}` – Delete brand  

### 📦 Orders  
- `POST /api/Orders` – Place new order  
- `GET /api/Orders/GetOrdersForSpecificUser` – Get all orders for current user  
- `GET /api/Orders/GetOrderForSpecificUser/{orderId}` – Get order details  
- `GET /api/Orders/GetDeliveryMethods` – Get delivery methods  
- `DELETE /api/Orders/{orderId}` – Cancel order  
- `GET /api/Orders/all` – Get all orders (Admin only)  

### 💳 Payment  
- `POST /api/Payment/{basketId}` – Process payment  
- `POST /api/Payment/Webhook` – Stripe webhook handler  

### 🛍️ Products  
- `GET /api/Products` – Get all products  
- `POST /api/Products` – Add new product  
- `GET /api/Products/{id}` – Get product by ID  
- `PUT /api/Products/{id}` – Update product  
- `DELETE /api/Products/{id}` – Delete product  

### 📂 Types  
- `GET /api/Types` – Get all product types  
- `POST /api/Types` – Add new type  
- `GET /api/Types/{id}` – Get type by ID  
- `PUT /api/Types/{id}` – Update type  
- `DELETE /api/Types/{id}` – Delete type  

---

## 🏗️ Architecture

The API follows **Onion Architecture**, ensuring clean separation of concerns and maintainable code.  

- **Core Layer** – Business logic and domain models  
- **Application Layer** – Services and application logic  
- **Infrastructure Layer** – Database, payment gateway, caching, etc.  
- **API Layer** – Entry point for HTTP requests  

### ✨ Design Patterns
- **Unit of Work** – Manages transactions and ensures consistency  
- **Generic Repository** – Simplified data access  
- **Specification Pattern** – Build complex queries dynamically  
- **AutoMapper** – Map domain models to DTOs easily  

---

## ⚡ Caching

**Redis** is integrated to cache frequently requested data (e.g., product lists) —  
this improves response times and reduces database load, which is critical for high-traffic e-commerce platforms.

---

## 🛡️ Error Handling

A global exception handling middleware provides meaningful error messages and consistent responses across the API.

---

## 💳 Stripe Integration

Secure payment processing is handled through **Stripe API** with webhook support to track payment events and status updates.

---

## 🐳 Docker Support

The entire application is **containerized using Docker**:  
- Built with a **multi-stage Dockerfile** for optimized image size.  
- Easily deployable across development, staging, and production environments.  
- Integrated with **GitHub Actions** for automated build and deployment pipelines.  

---

## ☁️ Deployment

The API is deployed and hosted on **MonsterAPI**.  
You can try the live version here:  

👉 [**Live Swagger UI**](https://webstorev.runasp.net/swagger/index.html)  
