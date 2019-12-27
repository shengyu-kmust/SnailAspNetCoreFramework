<template>
    <div class="ui-layout-container ui-layout-flex ui-layout-flex--column ui-height-100">
        <iframe :src="getImpotUrl" class="ui-height-100" frameborder="0"></iframe>
    </div>
</template>
<script>
export default {
    name: 'HtmlPanel',
    props: {
        url: {
            type: String,
            default: ''
        }
    },
    data() {
        return {
            importUrl: '',
            getUrl: ''
        };
    },
    computed: {
        getImpotUrl() {
            //this.importUrl = 'https://www.baidu.com/';
            //return this.importUrl;
            return this.getUrl + this.url;
        }
    },
    methods: {},
    created() {
        var code = {code: 'AocUrl'};
        this.$client.getConfigValueByCode(code).then(data => {
            this.getUrl = data;
        });
    }
};
</script>
