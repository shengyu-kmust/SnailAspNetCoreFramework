/**
 * 定义通用表格管理混入对象 BY JCY 2018-7-12
 * v0.1
 */
export const DataGridBaseMixin = {
  data() {
    // 读取通用配置
    const config = this.$tools.config

    return {
      dataGridConf: config.DataGridConfig(),
      ComponentSize: config.ComponentSize(),
      grid: {
        rows: [],
        total: 0
      },
      keyword: '',
      gridLoaded: false,
      currentRow: null,
      multipleSelectionType: false,
      multipleSelection: [],
      // 表格名字 定义在 ref
      DataGridName: null,
      SearchFnName: 'getAllUserExtra',
      DelFnName: 'delPersonnelAdditionalInfo',
      SubFormSaveFnName: 'submitForm',
      dialogVisible: false,
      formTitle: this.$t('common.new'),
      loadingHandle: {},
      formData: {},
      checkSelection: []
    }
  },
  mounted() {
    this.initloadGrid()
    this.CombingTableEvents()
  },
  methods: {
    // 全局loading层
    OpenLoading(_text) {
      this.loadingHandle = this.$loading({
        lock: true,
        text: _text || '拼命加载中...',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)'
      })
    },
    CloseLoading() {
      this.loadingHandle.close()
    },
    // 变化页码
    handleGridChange(val, dgConfName) {
      if (this.dataGridConf[dgConfName] !== val) {
        this.dataGridConf[dgConfName] = val
        this.Search(false)
      }
    },
    // 选中表格中的行
    selectRowChange(val, otherDataGridName) {
      this.currentRow = { ...val }
      this.doSelectRow(val, otherDataGridName)
    },
    doSelectRow(val, otherDataGridName) {
      try {
        // 和表格中的设备有关
        // <el-table-column
        //     type="selection"
        //     width="55"
        // ></el-table-column>
        let findR
        // let tableObj = (otherDataGridName || this.DataGridName) ? this.$refs[otherDataGridName || this.DataGridName] : this.$refs.table;
        const tableObj = this.$refs[this.DataGridName]
        // 以下代码是为了兼容 BasicManager.vue 不支持设置 this.DataGridName 属性的折中方案，弊端是一个 BasicManager 只可以支持一个表格
        if (tableObj && tableObj.columns.length > 0) {
          findR = tableObj.columns.findIndex(a => {
            return a.type === 'selection'
          })
          if (findR && findR !== -1) {
            tableObj.toggleRowSelection(val)
          }
        }
      } catch (e) {
        console.log(
          '设置选中表格中的哪几行错误, 有可能是 DataGridName 未定义',
        )
        console.log(e)
      }
    },
    // 设置选中表格中的哪几行
    toggleSelection(row) {
      try {
        if (row && this.$refs[this.DataGridName]) {
          this.$refs[this.DataGridName].toggleRowSelection(row)
        } else {
          this.$refs[this.DataGridName].clearSelection()
        }
      } catch (e) {
        console.log(
          '设置选中表格中的哪几行错误, 有可能是 DataGridName 未定义',
        )
        console.log(e)
      }
    },
    checkSelect(selection, row) {
      // this.selectRowChange(row, null);
      this.checkSelection = selection
    },
    checkSelectAll(selection) {
      this.checkSelection = selection
    },
    // 多选-选中表格中的行
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 查询
    Search(type) {
      this.currentRow = null
      if (type) {
        this.dataGridConf.pageIndex = 1
      }
      this.LoadGridData(this.dataGridConf, this.makeSearchPar())
    },
    // 加载数据
    LoadGridData(_gridPar, _searchPar) {
      this.OpenLoading()
      this.$client[this.SearchFnName](
        this.$tools.MakeDataGridSearchPart(_gridPar, _searchPar),
      )
        .then(ret => {
          this.CloseLoading()
          if (ret && ret.code && ret.data) {
            this.grid = this.$tools.GridDataAdapter(ret.data)
          } else {
            this.$message.error(
              '数据加载失败' + this.SearchFnName || '',
            )
          }
          this.LoadDataFinish()
        })
        .catch(err => {
          this.CloseLoading()
          this.ClientRequestError(this.SearchFnName)
        })
    },
    LoadDataFinish() {},
    // async LoadGridData(_gridPar, _searchPar) {
    //     this.OpenLoading();
    //     try {
    //         const ret = await this.$client[this.SearchFnName](this.$tools.MakeDataGridSearchPart(_gridPar, _searchPar));
    //         this.CloseLoading();
    //         if (ret && ret.code && ret.data) {
    //             this.grid = this.$tools.GridDataAdapter(ret.data);
    //         } else {
    //             this.$message.error('数据加载失败' + this.SearchFnName || '');
    //         }
    //     } catch (e) {
    //         this.CloseLoading();
    //         this.ClientRequestError(this.SearchFnName);
    //     }
    // },
    // 加载数据错误
    ClientRequestError(FnName) {},
    // 添加
    Add() {
      this.dialogVisible = true
      this.formTitle = this.$t('common.new')
      this.LoadFormData(false)
    },
    // 修改
    Edit() {
      if (this.multipleSelectionType) {
        if (
          this.multipleSelection &&
                    this.multipleSelection.length === 1
        ) {
          this.formTitle = this.$t('common.edit')
          this.dialogVisible = true
          this.currentRow = this._.cloneDeep(
            this.multipleSelection[0],
          )
          this.LoadFormData(true)
        } else {
          this.$alert(
            this.$t('common.select-row-data'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes')
            },
          )
        }
      } else {
        if (this.currentRow) {
          this.formTitle = this.$t('common.edit')
          this.dialogVisible = true
          this.LoadFormData(true)
        } else {
          this.$alert(
            this.$t('common.select-row-data'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes')
            },
          )
        }
        console.log('')
      }
    },
    // 加载表单数据
    LoadFormData(type) {},
    // 删除
    Delete() {
      if (this.multipleSelectionType) {
        const ids = this.getCheckboxIds()
        if (ids && ids.length > 0) {
          this.dialogVisible = false

          this.$confirm(
            this.$t('common.confirm.delete'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes'),
              cancelButtonText: this.$t('common.cancel'),
              type: 'warning'
            },
          )
            .then(() => {
              this.PerformDeleteRow(ids)
            })
            .catch(e => {
              console.log('base PerformDeleteRow error')
            })
        } else {
          this.$alert(
            this.$t('common.select-row-data'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes')
            },
          )
        }
      } else {
        if (
          (this.currentRow && this.currentRow.Id) ||
                    this.checkSelection.length > 0
        ) {
          this.dialogVisible = false
          this.$confirm(
            this.$t('common.confirm.delete'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes'),
              cancelButtonText: this.$t('common.cancel'),
              type: 'warning'
            },
          )
            .then(() => {
              this.PerformDeleteRow()
            })
            .catch(e => {
              console.log('base PerformDeleteRow error')
            })
        } else {
          this.$alert(
            this.$t('common.select-row-data'),
            this.$t('common.warning'), {
              confirmButtonText: this.$t('common.yes')
            },
          )
        }
        console.log('delete no multiple')
      }
    },
    // 执行删除操作
    PerformDeleteRow(ids) {
      this.OpenLoading('删除操作中，请稍等...')
      if (this.multipleSelectionType) {
        let newStr = ''
        if (ids && ids.length > 0) {
          for (var i = ids.length; i--;) {
            newStr += 'ids[' + i + ']=' + ids[i] + '&'
          }
          newStr += '_s=' + this.$tools.GetRandom()
        }
        this.$client[this.DelFnName](newStr)
          .then(ret => {
            this.CloseLoading()
            if (ret && ret.code) {
              this.checkSelection = []
              this.multipleSelection = []
              this.DelCallBack()
            } else {
              this.$message.error(ret.msg)
            }
          })
          .catch(err => {
            this.CloseLoading()
            this.ClientRequestError(this.DelFnName)
          })
      } else {
        this.$client[this.DelFnName]({
          Id: this.currentRow.Id
        })
          .then(ret => {
            this.CloseLoading()
            if (ret && ret.code) {
              this.checkSelection = []
              this.multipleSelection = []
              this.DelCallBack()
            } else {
              this.$message.error(ret.msg)
            }
          })
          .catch(err => {
            this.CloseLoading()
            this.ClientRequestError(this.DelFnName)
          })
      }
    },
    // 删除回调
    DelCallBack() {
      this.CloseLoading()
      this.$message({
        message: this.$t('msg.delete.success'),
        type: 'success',
        onClose: () => {
          this.Search(true)
        }
      })
    },
    // 调用子窗体保存方法
    DialogSave() {
      // 调用子组件的保存方法
      this.$refs.form[this.SubFormSaveFnName](this.SaveCallBack)
    },
    // 保存回调
    SaveCallBack(_ret, _sendData) {
      this.$message.success('保存成功！')
      this.Search(true)
      this.dialogVisible = false
    },
    // 关闭弹窗
    closeDialog() {
      this.dialogVisible = false
    },
    // 页面打开后自动查询
    initloadGrid() {
      if (!this.gridLoaded) {
        this.LoadGridData(this.dataGridConf, this.makeSearchPar())
        this.gridLoaded = true
      }
    },
    // 构建表格查询字符串
    makeSearchPar() {
      return {
        keyword: this.keyword
      }
    },
    // 分页索引计算方法
    indexMethod(index) {
      return (
        index +
                1 +
                (this.dataGridConf.pageIndex - 1) * this.dataGridConf.pageSize
      )
    },
    // 双击修改
    DbClickTable() {
      this.Edit()
    },
    // 自定义表格单元的内容
    cellStyle({ row, column, rowIndex, columnIndex }) {
      // if (columnIndex === 0) {
      //     return 'white-space:nowrap;';
      // }
      // 自适应列
    },
    // 多选时，取得所有选中的一列并返回数组 id
    getIds(colName) {
      const result = []
      const _colName = colName || 'Id'
      if (this.multipleSelection && this.multipleSelection.length > 0) {
        for (
          var i = 0, len = this.multipleSelection.length; i < len; i++
        ) {
          result.push(this.multipleSelection[i][_colName])
        }
      }
      return result
    },
    getCheckboxIds(colName) {
      const result = []
      const _colName = colName || 'Id'
      if (this.checkSelection && this.checkSelection.length > 0) {
        for (
          var i = 0, len = this.checkSelection.length; i < len; i++
        ) {
          result.push(this.checkSelection[i][_colName])
        }
      }
      return result
    },
    // 取得当前页所有行
    getAllRows() {
      let rows = []
      try {
        if (this.$refs[this.DataGridName]) {
          rows = this.$refs[this.DataGridName].data
        }
      } catch (e) {}
      return rows
    },
    // 梳理表格事件
    CombingTableEvents() {
      let tableObj
      let findR
      try {
        // 如果表格列中有，checkbox 时，双击修改事件，将设置为失效
        tableObj = this.DataGridName
          ? this.$refs[this.DataGridName]
          : this.$refs.table
        if (tableObj && tableObj.columns.length > 0) {
          findR = tableObj.columns.findIndex(a => {
            return a.type === 'selection'
          })
          if (findR !== null && findR !== -1) {
            tableObj._events['row-dblclick'] = null
            this.multipleSelectionType = true
          }
        }
      } catch (e) {
        console.log('CombingTableEvents error')
      }
    }
  }
}
