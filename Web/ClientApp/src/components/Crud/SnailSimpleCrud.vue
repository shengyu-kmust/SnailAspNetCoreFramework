<template>
  <div>
    <el-button @click="add">增加</el-button>
    <el-button @click="edit">修改</el-button>
    <el-button @click="remove">删除</el-button>
    <!-- 查询条件 -->
    <snail-search-form ref="searchForm" :fields="searchFields" :rules="searchRules" @search="search" />
    <!-- table分页 -->
    <snail-page-table
      ref="table"
      v-loading="loading"
      :rows="tableDatas"
      :fields="fields"
      :pagination="pagination"
      :multi-select="multiSelect"
      @search="search"
    ></snail-page-table>
    <!-- form表单 -->
    <el-dialog v-if="visible" :visible.sync="visible">
      <snail-form ref="form" :fields="fields" :init-form-data="formData" :rules="formRules"></snail-form>
      <template slot="footer">
        <el-button @click="submit">提交</el-button>
        <el-button @click="visible=false">取消</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script>
export default {
  props: {
    searchApi: {
      type: String,
      default: () => ('')
    },
    addApi: {
      type: String,
      default: () => ('')
    },
    editApi: {
      type: String,
      default: () => ('')
    },
    removeApi: {
      type: String,
      default: () => ('')
    },
    searchFields: {
      type: Array,
      default: () => ([])
    },
    fields: {
      type: Array,
      default: () => ([])
    },
    multiSelect: {
      type: Boolean,
      default: () => (false)
    },
    searchRules: {
      type: Object,
      default: () => ([])
    },
    formRules: {
      type: Object,
      default: () => ([])
    }
  },
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      visible: false,
      pagination: { currentPage: 1, pageSize: 15, total: 0 },
      loading: false
    }
  },
  computed: {

  },
  created() {

  },
  mounted() {
    this.search()
  },
  methods: {
    remove() {
      var data = {}
      var currentRow = this.$refs.table.currentRow
      var selection = this.$refs.table.selection
      console.log('删除-' + JSON.stringify(currentRow))
      if (this.multiSelect) {
        if (selection.length > 0) {
          data = {
            ids: selection.map(a => a.id)
          }
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      } else {
        if (currentRow.id) {
          data = {
            id: currentRow.id
          }
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      }
      console.log(data)
      this.$api[this.removeApi](data).then(res => {
        this.$message({
          message: '操作成功',
          type: 'success'
        })
        this.search()
      }).catch(res => {
        this.$message({
          message: '操作失败',
          type: 'error'
        })
      })
    },
    add() {
      this.submitApi = this.addApi
      this.formData = {}
      this.visible = true
    },
    edit() {
      if (this.multiSelect && this.$refs.table.selection.length > 1) {
        this.$message({
          message: '只能对一条数据进行修改',
          type: 'warning'
        })
        return
      }
      if (!this.multiSelect && !this.$refs.table.currentRow.id) {
        this.$message({
          message: '请选择数据后再进行修改',
          type: 'warning'
        })
        return
      }
      this.submitApi = this.editApi
      this.formData = this.$refs.table.currentRow
      console.log('edit-' + JSON.stringify(this.formData))
      this.visible = true
    },
    submit() {
      console.log(this.$refs.form.formData)
      this.$refs.form.validate(valid => {
        if (valid) {
          this.$api[this.submitApi](this.formData).then(res => {
            this.$message({
              message: '操作成功',
              type: 'success'
            })
            this.search()
          }).catch(res => {
            this.$message({
              message: '操作失败',
              type: 'error'
            })
          })
        }
      })
    },
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          var pagination = this.$refs.table.getPagination()
          var queryData = Object.assign({}, serachForm, pagination)
          console.log(queryData)
          this.loading = true
          this.$api[this.searchApi](queryData).then(res => {
            this.pagination.total = parseInt(res.data.total) || 0
            this.pagination.pageSize = parseInt(res.data.pageSize) || 15
            this.pagination.currentPage = parseInt(res.data.currentPage) || 1
            this.tableDatas = res.data.items
          }).finally(() => {
            this.loading = false
          })
        }
      })
    }
  }
}
</script>
