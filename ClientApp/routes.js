import HomePage from 'components/home-page'
import Items from 'components/items'
import AddItem from 'components/add-item'
import Locations from 'components/locations'
import ItemTemplates from 'components/item-templates'
import AddTemplate from 'components/add-template'
import Test from 'components/test-page'
import Faq from 'components/faq-page'
import Report from 'components/report'
import ViewEvents from 'components/view-events'
import Teachers from 'components/teachers'
import AddTeacher from 'components/add-teacher'
import TeacherBarcodes from 'components/teacher-barcodes-page'
import EditTeacher from 'components/edit-teacher-page'
import ScanPage from 'components/scan-page'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna', icon: "home" },
    { path: '/messages', component: ScanPage, display: "Wiadomości", icon: "envelope" },
    { path: '/items', component: Items, display: 'Wyposażenie', icon: "clipboard list" },
    { path: '/add-item', component: AddItem },
    { path: '/locations', component: Locations, display: 'Położenia', icon: "map marker alternate" },
    { path: '/item-templates', component: ItemTemplates, display: 'Typy przedmiotów', icon: "wrench" },
    { path: '/add-template', component: AddTemplate },
    { path: '/report', component: Report, display: 'Raporty', icon: "clipboard check" },
    { path: '/view-events/:id', component: ViewEvents },
    { path: '/teachers', component: Teachers, display: 'Nauczyciele', icon: "user" },
    { path: '/add-teacher', component: AddTeacher },
    { path: '/faq-page', component: Faq, display: 'Pomoc', icon: "question circle" },
    { path: '/teacher-barcodes/:id', component: TeacherBarcodes },
    { path: '/edit-teacher/:id', component: EditTeacher },
    { path: '/scan', component: ScanPage, display: "Skanowanie", icon: "play" },
]

