<template>
<div>
    <h1>Pomoc</h1>

    <div class="ui styled fluid accordion">
        <div class="title">
            <i class="dropdown icon"></i>
            Do czego służy ta strona?
        </div>
        <div class="content">
            <p class="transition hidden">
                Strona, na której się znajdujesz, służy do zarządzania inwentarzem.
                TODO
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Czym są położenia przedmiotów?
        </div>
        <div class="content">
            <p class="transition hidden">
                Położeniem przedmiotu jest miejsce, w którym przedmiot ma być docelowo przechowywany.
                Przy pobraniu przedmiotu przez nauczyciela, jego położenie nie ulega zmianie.
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Czym są typy przedmiotów?
        </div>
        <div class="content">
            <p class="transition hidden">
                Aby uniknąć powielania informacji przy każdym dodanym przedmiocie, dostępne są typy przedmiotów.
                Każdy dodany przedmiot posiada swój typ, który zawiera nazwę przedmiotu oraz jego opis.
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Jak wygląda procedura rejestracji przedmiotów?
        </div>
        <div class="content">
            <p class="transition hidden">
                Aby zarejestrować przedmioty, należy udać się do zakładki <i>Wyposażenie</i>, a następnie kliknąć w przycisk <i>Dodaj przedmiot</i>.
                Teraz trzeba wybrać położenie oraz typ przedmiotu. Możliwe jest dodanie więcej niż jednego przedmiotu naraz.
                Istnieje również możliwość szybkiego wprowadznia identyfikatorów dla nowych przedmiotów, wówczas należy zaznaczyć opcję <i>Wprowadź identyfikatory od razu</i>.<br>
                W ten sam sposób można wprowadzić identyfikatory do wcześniej dodanych przedmiotów. By to zrobić, należy użyć przycisku <i>Uzupełnij identyfikatory</i> w zakładce <i>Wyposażenie</i>,
                zaznaczyć przedmioty, których identyfikatory chcemy uzupełnić, a następnie klikając w przycisk <i>Uzupełnij identyfikatory</i> jeszcze raz.
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Jak wygląda procedura pobrania i zwrotu przedmiotu?
        </div>
        <div class="content">
            <p class="transition hidden">
                Nauczyciel, który zamierza pobrać dany przedmiot, najpierw skanuje skanerem swój kod kreskowy pobrania, a następnie skanuje kod przedmiotu. Zeskanowanie kodu pobrania następuje tylko raz.<br>
                Przy zwrocie przedmiotu należy zeskanować kod zwrotu oraz kod przedmiotu.
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Czym są role?
        </div>
        <div class="content">
            <p class="transition hidden">
                Role określają jakie działania użytkownik może na stronie podjąć.<br>
                Administrator ma dostęp do wszystkich opcji.<br>
                Moderator jest rolą pozwalającą na zarządzanie wyposażeniem, ale bez możliwości zarządzania użytkownikami.<br>
                Zwykły użytkownik jest w stanie jedynie przejrzeć listę przedmiotów, bez możliwości jej edytowania.
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Kto jest twórcą strony?
        </div>
        <div class="content">
            <p class="transition hidden">
                <div class="ui items">
                    <div class="item">
                        <a class="ui tiny image">
                            <img src="http://url.szymekk.me/py2jl" alt="">
                        </a>
                        <div class="middle aligned content">
                            <div class="header">
                                Bronisław Kwiotek
                            </div>
                            <div class="description">
                                Koordynator projektu
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <a class="ui tiny image">
                            <img src="https://avatars0.githubusercontent.com/u/7378222?s=400&u=d34502fdd67f698a841c60c00a0b78d261f04202&v=4">
                        </a>
                        <div class="middle aligned content">
                            <div class="header">
                                Szymon Jankowski
                            </div>
                            <div class="description">
                                Frontend oraz Backend
                            </div>
                        </div>
                    </div>
                </div>
            </p>
        </div>

        <div class="title">
            <i class="dropdown icon"></i>
            Lista zmian
        </div>
        <div class="content">
            <p class="transition hidden">
                <div class="ui relaxed divided list">
                    <div class="item">
                        <div class="content">
                            <div class="header">08.06.2018</div>
                            <div class="description">Dodano statystyki na stronie głównej</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">07.06.2018</div>
                            <div class="description">Dodano stronę szybkiego uzupełniania identyfikatorów</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">02.06.2018</div>
                            <div class="description">Dodano role oraz możliwość resetowania hasła przy pomocy wiadomości e-mail</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">01.06.2018</div>
                            <div class="description">Dodano podstronę <i>Moje konto</i> oraz system wysyłania wiadomości e-mail</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">23.05.2018</div>
                            <div class="description">Dodano opcje pobierania i zwracania przedmiotów przy pomocy skanera kodów kreskowych</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">20.05.2018</div>
                            <div class="description">Dodano tę informację</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="content">
                            <div class="header">15.01.2018</div>
                            <div class="description">Rozpoczęto prace nad systemem</div>
                        </div>
                    </div>
                </div>
            </p>
        </div>
    </div>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            items: [ ],
            searchText: "",
        };
    },

    methods: {
        filterItems(items) {
            return items.filter((i) => this.searchText.length == 0 || (i.name + i.description + i.location).toLowerCase().includes(this.searchText.toLowerCase()));
        },
        goToAddItem() {
            router.push("/add-item");
        },
        async removeItem(id) {
            await this.$http.post('/api/Items/Remove', "id=" + id);
            let response = await this.$http.get('/api/Items')
            this.items = response.data;
        }
    },

    computed: {
        
    },

    mounted() {
        $('.ui.accordion').accordion();
    },

    async created() {
        let response = await this.$http.get('/api/Items')
        this.items = response.data;
    }
};
</script>

<style>

</style>
