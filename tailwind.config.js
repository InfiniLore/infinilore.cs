/** @type {import('tailwindcss').Config} */
const plugin = require('tailwindcss/plugin')

module.exports = {
    content: [
        "./**/*.{razor,html,cshtml}"
    ],
    theme: {
        extend: {
        },
    },
    variants: {
        extend: {
            width: ['responsive', 'hover', 'focus'],
        },
    },
    plugins: [
    ],
}