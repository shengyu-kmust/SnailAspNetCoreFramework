<template>
  <!-- 不分页的CRUD  -->
  <div style="height:100%">
    <el-button @click="add">增加</el-button>
    <el-button @click="edit">修改</el-button>
    <el-button @click="remove">删除</el-button>
    <slot></slot>
    <!-- 查询条件 -->
    <snail-search-form v-show="searchFields.length>0" ref="searchForm" :fields="searchFields" :rules="searchRules" @search="search" />
    <!-- table分页 -->
    <snail-table
      ref="table"
      v-loading="loading"
      :rows="tableDatas"
      :fields="fields"
      :multi-select="multiSelect"
      :height="500"
      @search="search"
    ></snail-table>
    <!-- form表单 -->
    <el-dialog v-if="visible" :visible.sync="visible">
      <snail-form ref="form" :fields="formFields" :init-form-data="formData" :rules="formRules"></snail-form>
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
    formFields: {
      type: Array,
      default: () => ([])
    },
    multiSelect: {
      type: Boolean,
      default: () => (false)
    },
    searchRules: {
      type: Object,
      default: () => ({})
    },
    formRules: {
      type: Object,
      default: () => ({})
    },
    beforeSearch: {
      type: Function,
      default: () => {}
    },
    beforeSubmit: {
      type: Function,
      default: () => {}
    }
  },
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      visible: false,
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
      var ids = []
      var currentRow = this.$refs.table.currentRow
      var selection = this.$refs.table.selection
      console.log('删除-' + JSON.stringify(currentRow))
      if (this.multiSelect) {
        if (selection.length > 0) {
          ids = selection.map(a => a.id)
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      } else {
        if (currentRow.id) {
          ids = [currentRow.id]
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      }
      this.$api[this.removeApi](ids).then(res => {
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
      this.visible = true
    },
    submit() {
      this.$refs.form.validate(valid => {
        if (valid) {
          if (typeof this.beforeSubmit === 'function') {
            var isContinue = this.beforeSubmit(this.$refs.form.formData)
            if (isContinue === false) {
              return
            }
          }
          this.$api[this.submitApi](this.$refs.form.formData).then(res => {
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
          this.visible = false
        }
      })
    },
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          this.loading = true
          if (typeof this.beforeSearch === 'function') {
            var isContinue = this.beforeSearch(serachForm)
            if (isContinue === false) {
              return
            }
          }
          this.$api[this.searchApi](serachForm).then(res => {
            this.tableDatas = res.data
          }).finally(() => {
            this.loading = false
          })
        }
      })
    }
  }
}
</script>
