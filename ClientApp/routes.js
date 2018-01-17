import HomePage from 'components/home-page'
import Items from 'components/items'
import AddItem from 'components/add-item'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna' },
    { path: '/items', component: Items, display: 'Wyposażenie' },
    { path: '/add-item', component: AddItem, display: 'Dodaj przedmiot' },
]
