import HomePage from 'components/pages/home-page'
import Items from 'components/pages/items'
import AddItem from 'components/pages/add-item'
import Locations from 'components/pages/locations'
import ItemTemplates from 'components/pages/item-templates'
import AddTemplate from 'components/pages/add-template'
import Test from 'components/pages/test-page'
import Faq from 'components/pages/faq-page'
import Report from 'components/pages/report'
import ViewEvents from 'components/pages/view-events'
import Teachers from 'components/pages/teachers'
import AddTeacher from 'components/pages/add-teacher'
import TeacherBarcodes from 'components/pages/teacher-barcodes-page'
import EditTeacher from 'components/pages/edit-teacher-page'
import ScanPage from 'components/pages/scan-page'
import MessagesPage from 'components/pages/messages-page'
import ViewMessagePage from 'components/pages/view-message-page'
import MyAccountPage from 'components/pages/my-account-page'

export const routes = [
    { path: '/', component: HomePage, display: 'Strona główna', icon: "home" },
    { path: '/messages', component: MessagesPage, display: "Wiadomości", icon: "envelope" },
    { path: '/view-message/:id', component: ViewMessagePage },
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
    { path: '/my-account', component: MyAccountPage, display: "Moje konto", icon: "address card" },
]

