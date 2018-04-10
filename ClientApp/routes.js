import HomePage from 'components/home-page'
import Items from 'components/items'
import AddItem from 'components/add-item'
import Locations from 'components/locations'
import ItemTemplates from 'components/item-templates'
import AddTemplate from 'components/add-template'
import Test from 'components/test-page'
<<<<<<< HEAD
import Report from 'components/report'
=======
import ViewEvents from 'components/view-events'
>>>>>>> 19a4538962d6a3a7a8d2d226107430664f63cb3e

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna' },
    { path: '/items', component: Items, display: 'Wyposażenie' },
    { path: '/add-item', component: AddItem, display: 'Dodaj przedmiot' },
    { path: '/locations', component: Locations, display: 'Położenia' },
    { path: '/item-templates', component: ItemTemplates, display: 'Typy przedmiotów' },
    { path: '/add-template', component: AddTemplate, display: 'Dodaj typ przedmiotu' },
<<<<<<< HEAD
    { path: '/report', component: Report, display: 'Raporty' },
    { path: '/test-page', component: Test, display: 'Test' }
=======
    { path: '/test-page', component: Test, display: 'Test' },
    { path: '/view-events/:id', component: ViewEvents}
>>>>>>> 19a4538962d6a3a7a8d2d226107430664f63cb3e
]
