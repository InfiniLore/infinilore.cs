/** @type {import('tailwindcss').Config} */
const plugin = require('tailwindcss/plugin')

module.exports = {
    content: [
        "./**/*.{razor,html,cshtml,cs}"
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