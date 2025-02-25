import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: /* process.env.VITE_API_BASE_URL */ 'http://localhost:5158', // remove .env file and it will work
        changeOrigin: true,
        secure: false, // DO NOT USE IN PRODUCTION. GET A VALID SSL CERTIFICATE
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
})
