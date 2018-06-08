<template>
<div>
    <h1 class="six wide column">Wprowadzanie identyfikatorów</h1>

    <error-display/>

    <div class="ui three column grid">
        <div class="column">
            <button class="ui labeled icon button" :disabled="currentItemIndex == 0" @click="previousItem"><i class="left arrow icon"></i> Poprzedni</button>
        </div>
        <div class="column">
            <p class="current-item-text">Przedmiot {{ currentItemIndex + 1 }}/{{ itemIds.length }}</p>
        </div>
        <div class="column">
            <template v-if="currentItemIndex == itemIds.length - 1">
                <button class="ui right floated primary right labeled icon button" @click="finish">Gotowe<i class="check icon"></i></button>
            </template>
            <template v-else>
                <button class="ui right floated right labeled icon button" @click="nextItem">Następny<i class="right arrow icon"></i></button>
            </template>
        </div>
    </div>

    <div class="ui divider"></div>

    <template v-if="currentItem">
        <h2 class="name-text">Wprowadź identyfikator dla przedmiotu <i>{{ currentItem.name }} <template v-if="currentItem.locationName && currentItem.locationName != ''">({{ currentItem.locationName }})</template></i></h2>

        <div class="ui centered two column grid">
            <div class="column">
                <div class="ui form">
                    <div class="field">
                        <input class="name-text" v-model="identifier" type="text" placeholder="Tu wprowadź kod kreskowy lub użyj skanera kodów" id="identifierInput">
                    </div>
                    <button class="ui green fluid button" :class="{ 'loading': loading }" @click="acceptCurrentItem" id="#acceptButton">Zatwierdź</button>
                </div>
            </div>
        </div>
    </template>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            loading: false,

            itemIds: this.$route.params.itemIds || [],
            currentItemIndex: 0,

            currentItem: null,

            identifier: "",
        };
    },

    methods: {
        async updateItemInfo() {
            try {
                this.currentItem = await this.api.getItem(this.itemIds[this.currentItemIndex]);
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
        },

        async acceptCurrentItem() {
            if(this.loading)
                return;

            this.loading = true;

            this.api.clearError();

            var errored = false;

            try {
                await this.api.updateItemIdentifier(this.currentItem.id, this.identifier);
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
                errored = true;
            }

            this.identifier = "";
            $("#identifierInput").val("");
            $("#identifierInput").select();

            this.loading = false;

            if(!errored)
                await this.nextItem();
        },

        async previousItem() {
            if(this.currentItemIndex == 0)
                return;

            this.currentItemIndex--;

            await this.updateItemInfo();
        },

        async nextItem() {
            this.currentItemIndex++;
            
            if(this.currentItemIndex == this.itemIds.length) {
                router.push("/items");
                return;
            }

            await this.updateItemInfo();
        },

        async finish() {
            router.push("/items");
        }
    },

    updated() {
        $("#identifierInput").keyup(ev => {
            if(ev.keyCode === 13)
                this.acceptCurrentItem();
        });
    },

    async created() {
        this.api.loading = true;

        if(!this.itemIds.length || this.itemIds.length == 0) {
            this.api.displayError("Wystąpił błąd", "Spróbuj jeszcze raz.");
            this.api.loading = false;
            router.push("/items");
            return;
        }

        await this.updateItemInfo();

        $("#identifierInput").select();

        this.api.loading = false;
    },
};
</script>

<style>
    .current-item-text {
        font-size: 24px;
        text-align: center;
    }

    .name-text {
        text-align: center;
    }
</style>
