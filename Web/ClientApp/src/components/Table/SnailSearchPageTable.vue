<template>
  <div>
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
  </div>
</template>

<script>
export default {
  props: {
    searchApi: {
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
    }
  },
  data() {
    return {
      tableDatas: [],
      visible: false,
      pagination: { pageIndex: 1, pageSize: 15, total: 0 },
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
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          var pagination = this.$refs.table.getPagination()
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
    }
  }
}
</script>
