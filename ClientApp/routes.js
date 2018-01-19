import HomePage from 'components/home-page'
import Items from 'components/items'
import AddItem from 'components/add-item'
import Locations from 'components/locations'
import ItemTemplates from 'components/item-templates'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna' },
    { path: '/items', component: Items, display: 'Wyposażenie' },
    { path: '/add-item', component: AddItem, display: 'Dodaj przedmiot' },
    { path: '/locations', component: Locations, display: 'Położenia' },
    { path: '/item-templates', component: ItemTemplates, display: 'Typy Przedmiotow' }
]
