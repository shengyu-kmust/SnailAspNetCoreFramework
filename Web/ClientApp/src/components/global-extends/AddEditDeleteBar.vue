<template>
    <el-row>
        <el-col :span="24">
            <el-button v-if="$auth.check(auth.add)" :size="size" type="primary" plain icon="el-icon-plus" @click="$emit('add')">
                {{$t("common.new")}}
            </el-button>
            <el-button v-if="$auth.check(auth.edit)" :size="size" type="primary" plain icon="el-icon-edit" @click="$emit('edit')">
                {{$t("common.edit")}}
            </el-button>
            <el-button v-if="$auth.check(auth.delete)" :size="size" type="danger" plain icon="el-icon-delete" @click="$emit('delete')">
                {{$t("common.delete")}}
            </el-button>
        </el-col>
    </el-row>
</template>

<script>
export default {
    name: 'AddEditDeleteBar',
    props: {
        size: String,
        auth: {
            type: Object,
            default: () => ({
                add: '',
                edit: '',
                delete: ''
            })
        }
    }
};
</script>

<style scoped>
</style>
