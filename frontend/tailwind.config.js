/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        brand: {
          DEFAULT: "#0E4C75", // deep blue (primary)
          dark: "#0D3F63",
        },
        sage: "#6FAF8E", // secondary accent
        charcoal: "#2E3A44", // tertiary
      },
    },
  },
  plugins: [],
};
