<template>
    <el-row :gutter="20">
        <el-col :span="4">
            <el-input :size="size" :placeholder="$t('placeholder.search')" clearable v-model="form.keyword">
            </el-input>
        </el-col>
        <el-col :span="4">
            <el-button :size="size" type="primary" plain icon="el-icon-search" :span="1" @click="$emit('search',form)">
                {{$t("common.search")}}
            </el-button>
        </el-col>
    </el-row>
</template>

<script>
    export default {
        name: "SearchBar",
        props:['size'],
        data(){
            return{
                form:{
                    keyword:''
                }
            }
        }
    }
</script>

<style scoped>

</style>
