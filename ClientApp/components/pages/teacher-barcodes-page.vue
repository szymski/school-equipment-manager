<template>
<div>
    <h1 class="six wide column">Kod kreskowy nauczyciela <span v-if="teacher"> - {{ teacher.name }} {{ teacher.surname }}</span></h1>

    <error-display/>

    <div v-if="barcodes">
        <div v-for="(item, key) in barcodes" :key="key" class="ui segments">
            <div class="ui secondary segment">
                <h3 class="barcode-title">
                    {{ ({ base: "Kod nauczyciela", borrow: "Kod pobrania", return: "Kod zwrotu" })[key] }}
                </h3>
            </div>
            <div class="ui segment">
                <img class="barcode" :src="'/api/BarCode/Generate?text=' + item"/>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            teacher: null,

            barcodes: null,
        };
    },

    methods: {
        
    },

    async created() {
        this.api.loading = true;

        try {
            this.teacher = await this.api.getTeacher(this.$route.params.id);
            this.barcodes = await this.api.getBarcodesForTeacher(this.teacher.id);
            this.barcodes = {
                base: this.barcodes.base,
                borrow: "POBR",
                return: "ZWROT",
            };
        }
        catch(e) {
            this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
        }

        this.api.loading = false;
    }
};
</script>

<style>
    .barcode-title {
        text-align: center;
    }

    .barcode {
        display: block;
        height: 180px;
        text-align: center;
        margin: auto;
    }
</style>
