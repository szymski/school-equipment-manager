import HomePage from 'components/home-page'
import Items from 'components/items'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna' },
    { path: '/items', component: Items, display: 'Wyposażenie' },
]
