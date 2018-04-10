import HomePage from 'components/home-page'
import Items from 'components/items'
import AddItem from 'components/add-item'
import Locations from 'components/locations'
import ItemTemplates from 'components/item-templates'
import AddTemplate from 'components/add-template'
import Test from 'components/test-page'
import Report from 'components/report'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna' },
    { path: '/items', component: Items, display: 'Wyposażenie' },
    { path: '/add-item', component: AddItem, display: 'Dodaj przedmiot' },
    { path: '/locations', component: Locations, display: 'Położenia' },
    { path: '/item-templates', component: ItemTemplates, display: 'Typy przedmiotów' },
    { path: '/add-template', component: AddTemplate, display: 'Dodaj typ przedmiotu' },
    { path: '/report', component: Report, display: 'Raporty' },
    { path: '/test-page', component: Test, display: 'Test' }
]
