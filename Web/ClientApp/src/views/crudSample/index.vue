<template>
  <div>
    <el-button @click="add">增加</el-button>
    <el-button @click="edit">修改</el-button>
    <snail-table :data="tableDatas" :columns="columns">

    </snail-table>
    <el-dialog :visible="visible">
      <snail-form :fields="columns">

      </snail-form>
    </el-dialog>

  </div>
</template>

<script>
export default {
  data() {
    return {
      tableDatas: [],
      columns: [
        {
          fieldName: 'string',
          label: 'string'
        },
        {
          fieldName: 'int',
          label: 'int'
        },
        {
          fieldName: 'datetime',
          label: 'datetime'
        },
        {
          fieldName: 'select',
          label: 'select'
        },
        {
          fieldName: 'multiselect',
          label: 'multiselect'
        }
      ],
      visible: false
    }
  },
  created() {
    this.$api.getList().then(res => {
      this.tableDatas = res.data
    })
  },
  methods: {
    add() {
      this.visible = true
    },
    edit() {
      this.visible = true
    }
  }
}
</script>
