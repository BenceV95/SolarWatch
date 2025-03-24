# SolarWatch

SolarWatch is a React-based web application that allows users to track sunrise, sunset, solar noon, and day length for a given location and date. The project is built using Vite for fast development and includes features such as user authentication, protected routes, and an admin panel for managing data.

## Features

- **User Authentication**: Login and register functionality with JWT-based authentication.
- **Solar Data Tracking**: View sunrise, sunset, solar noon, and day length for a specific location and date.
- **Admin Panel**: Manage location and solar data (delete functionality included).
- **Responsive Design**: Styled with CSS for a clean and user-friendly interface.
- **Protected Routes**: Role-based access control for user and admin routes.
- **Docker Support**: Easily deployable using Docker.

## Project Structure

```
src/
├── App.jsx               # Main application component
├── components/           # Reusable components
│   ├── AdminPanel/       # Admin panel components
│   ├── Footer/           # Footer component
│   ├── Home/             # Home page component
│   ├── Login/            # Login page component
│   ├── Navbar/           # Navigation bar component
│   ├── NotFound/         # 404 page component
│   ├── ProtectedRoute/   # Protected route logic
│   ├── Register/         # Registration page component
│   └── SolarWatch/       # Solar data tracking component
├── assets/               # Static assets (e.g., SVGs)
├── App.css               # Global styles
├── main.jsx              # Application entry point
└── index.css             # Global CSS variables and styles
```

## Installation

## Docker Deployment

1. Use Docker Compose to build:
```powershell
docker compose build
```

2. Then run:
```powershell
docker compose up -d
```

## Scripts

- `npm run dev`: Start the development server.
- `npm run build`: Build the project for production.
- `npm run preview`: Preview the production build.
- `npm run lint`: Run ESLint to check for code issues.

## Technologies Used

- **Frontend**: React, React Router, Vite
- **Styling**: CSS
- **State Management**: React Hooks
- **Authentication**: JWT
- **Deployment**: Docker, Nginx

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- SVG icons are sourced from [svgrepo.com](https://www.svgrepo.com/).
- This is a project for me to showcase my full stack skills.