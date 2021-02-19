/**
 * 定义通用简单的crud混入对象
 */
export const simpleCrudBaseMixin = {
  props: {
    autoLoad: {
      type: Boolean,
      default: true
    },
    hasPagination: {
      type: Boolean,
      default: true
    },
    showOper: {
      type: Boolean,
      default: true
    },
    showSearch: {
      type: Boolean,
      default: true
    },
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
    formFields: {
      type: Array,
      default: () => ([])
    },
    searchRules: {
      type: Object,
      default: () => ({})
    },
    formRules: {
      type: Object,
      default: () => ({})
    },
    initAddFormData: {
      type: Function,
      default: () => { }
    },
    beforeSearch: {
      type: Function,
      default: () => { }
    },
    afterSearch: {
      type: Function,
      default: () => { }
    },
    beforeSubmit: {
      type: Function,
      default: () => { }
    },
    handSearchTableDatas: {
      type: Function,
      default: null
    },
    // 下面是snailPageTable里的Props
    pagination: {
      type: Object,
      default: () => {
        return {
          pageIndex: 1,
          pageSize: 15,
          total: 0
        }
      }
    },
    pageSizes: {
      type: Array,
      default: () => ([15, 30, 50, 100, 200])
    },
    layout: {
      type: String,
      default: () => ('total, prev, pager, next, jumper, sizes')
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
  mounted() {
    if (this.autoLoad) {
      this.search()
    }
  },
  methods: {
    remove() {
      debugger;
      var ids = []
      var currentRow = this.currentRow
      var selection = this.selection
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
      } else if (currentRow.id) {
        ids = [currentRow.id]
      } else {
        this.$message({
          message: '请先选择要删除的数据',
          type: 'warning'
        })
        return
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
      if (typeof (this.initAddFormData) === 'function') {
        this.initAddFormData(this.formData)
      }
      this.visible = true
    },
    edit() {
      if (this.multiSelect && this.selection.length > 1) {
        this.$message({
          message: '只能对一条数据进行修改',
          type: 'warning'
        })
        return
      }
      if (!this.multiSelect && !this.currentRow.id) {
        this.$message({
          message: '请选择数据后再进行修改',
          type: 'warning'
        })
        return
      }
      this.submitApi = this.editApi
      this.formData = this.currentRow
      this.visible = true
    },
    submit() {
      debugger;
      const { submitHandler } = this
      if (submitHandler) {
        submitHandler()
      } else {
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
            })
            this.visible = false
          }
        })
      }
    },
    search() {
      if (this.searchApi) {
        if (this.hasPagination) {
          this.$refs.searchForm.validate(valid => {
            if (valid) {
              var serachForm = this.$refs.searchForm.formData
              var pagination = this.getPagination()
              if (typeof this.beforeSearch === 'function') {
                var isContinue = this.beforeSearch(serachForm)
                if (isContinue === false) {
                  return
                }
              }
              var queryData = Object.assign({}, serachForm, pagination)
              this.loading = true
              this.$api[this.searchApi](queryData).then(res => {
                this.pagination.total = parseInt(res.data.total) || 0
                this.pagination.pageSize = parseInt(res.data.pageSize) || 15
                this.pagination.pageIndex = parseInt(res.data.pageIndex) || 1
                this.tableDatas = res.data.items
              }).finally(() => {
                this.loading = false
              })
            }
          })
        } else {
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
                if (typeof this.handSearchTableDatas === 'function') {
                  var data = this.handSearchTableDatas(res)
                  if (data) {
                    res.data = data
                  }
                }

                if (typeof this.afterSearch === 'function') {
                  this.afterSearch()
                }
                this.tableDatas = res.data
              }).finally(() => {
                this.loading = false
              })
            }
          })
        }

      }
    },
     // 这个是SnailPageTable里的方法
     getPagination() {
      return {
        pageIndex: this.$refs.pagination.internalCurrentPage,
        pageSize: this.$refs.pagination.internalPageSize
      }
    },
    emitEventHandler(event) { // 对所有table的事件进行监听，并向父组件抛事件，//在tableBase.js里已经有了，怎么合并？ todo
      if (['pagination-size-change', 'pagination-current-change', 'pagination-next-click', 'pagination-prev-click'].indexOf(event) > -1) {
        this.search()
      }
      if (event === 'row-click') { // 点击
        var table = this.$refs[this.tableRefName]
        table.toggleRowSelection(Array.from(arguments)[1])
      }
      if (event === 'current-change') { // 单选点击
        this.currentRow = Array.from(arguments)[1]
      }

      if (event === 'selection-change') { // 多选点击
        this.selection = Array.from(arguments)[1]
      }

      this.$emit(event, ...Array.from(arguments).slice(1))// 所有事件往上抛，让外部也能监听
    }
  }
}
