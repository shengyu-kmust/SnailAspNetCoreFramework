<template>
  <div style="height:100%;width:100%">
    <!-- element 2.8.2及以上才有树table -->
    <snail-simple-list-crud
      ref="table"
      :show-table-index="false"
      search-api="resourceQueryListTree"
      add-api="resourceSave"
      edit-api="resourceSave"
      remove-api="resourceRemove"
      :fields="fields"
      :table-bind="tableBind"
      :form-fields="fields"
      :before-submit="beforeSubmit"
      @current-change="currentChange"
    >
      <template slot="oper">
        <div>
          <el-row>
            <el-button @click="addParent">添加同级</el-button>
            <el-button @click="addChild">添加子级</el-button>
            <el-button @click="edit">编辑</el-button>
            <el-button @click="remove">删除</el-button>
            <el-button @click="refresh">刷新</el-button>
          </el-row>
        </div>
      </template>
    </snail-simple-list-crud>
  </div>
</template>

<script>
export default {
  data() {
    return {
      keyValues: {
        genders: []
      },
      currentRow: {},
      addParentId: '',
      rootId: '',
      tableData: [],
      fields: [
        {
          name: 'name',
          type: 'string',
          label: '资源名'
        },
        {
          name: 'code',
          type: 'string',
          label: '资源code'
        }
      ],
      tableBind: {
        rowKey: 'id',
        defaultExpandAll: true
      }
    }
  },
  computed: {
  },
  created() {
  },
  methods: {
    currentChange(row) {
      this.currentRow = row
    },
    addParent() {
      if (this.currentRow && this.currentRow.parentId) {
        this.addParentId = this.currentRow.parentId
      } else {
        this.addParentId = this.rootId
      }
      console.log(this.addParentId)
      this.$refs.table.add()
    },
    refresh() {
      this.$refs.table.search()
    },
    addChild() {
      this.addParentId = this.currentRow.id
      if (!this.addParentId) {
        this.$message.error('请先选择数据，再对其增加子级')
        return
      }
      console.log(this.addParentId)
      this.$refs.table.add()
    },
    beforeSubmit(formData) {
      formData.parentId = this.addParentId
    },
    edit() {
      this.addParentId = this.currentRow.parentId
      this.$refs.table.edit()
    },
    remove() {
      this.$refs.table.remove()
    }
  }
}
</script>
