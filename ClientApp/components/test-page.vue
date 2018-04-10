<template>
<div>
    <h1>Test</h1>

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
                    <div class="item">
                        <a class="ui tiny image">
                            <img src="https://avatars0.githubusercontent.com/u/15158339?s=400&v=4">
                        </a>
                        <div class="middle aligned content">
                            <div class="header">
                                Bartek Kurpanik
                            </div>
                            <div class="description">
                                Backend oraz motywacja
                            </div>
                        </div>
                    </div>
                    <div class="item">
                        <a class="ui tiny image">
                            <img src="http://url.szymekk.me/i6cvw">
                        </a>
                        <div class="middle aligned content">
                            <div class="header">
                                Linus Torvalds
                            </div>
                            <div class="description">
                                Bóg. Stworzył Gita, a bez niego dostałbym zawału.
                            </div>
                        </div>
                    </div>
                </div>
            </p>
        </div>
    </div>
</div>
</template>

<script>
import router from "../router.js";

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
