<template>
    <el-main>
        <slot name="btnGroup">
            <add-edit-delete-bar
                :auth="btnAuth"
                :size="componentSize"
                @add="Add"
                @edit="Edit"
                @delete="Delete"
            ></add-edit-delete-bar>
        </slot>
        <slot name="searchBar">
            <search-bar :size="componentSize" @search="search"></search-bar>
        </slot>
        <el-table
            class="table"
            ref="table"
            :cell-style="cellStyle"
            :size="ComponentSize"
            :stripe="true"
            @select="checkSelect"
            @select-all="checkSelectAll"
            :border="true"
            :fit="true"
            :data="grid.items"
            @row-click="selectRowChange"
            @selection-change="handleSelectionChange"
            :highlight-current-row="HighlightCurrentRow"
            @row-dblclick="editRow"
            :style="elTableBmStyle"
        >
            <el-table-column v-if="showIndexColumn" type="index" label="序号" :index="indexMethod">
                <!--<div class="data-grid-index">{{indexMethod(scope.$index)}}</div>-->
            </el-table-column>
            <slot name="column"></slot>
        </el-table>
        <el-pagination
            @size-change="handleGridChange($event, 'pageSize')"
            @current-change="handleGridChange($event, 'pageIndex')"
            :current-page="dataGridConf.pageIndex"
            :page-sizes="dataGridConf.pageSizeRule"
            :page-size="dataGridConf.pageSize"
            layout="total, sizes, prev, pager, next, jumper"
            :total="grid.totalItemCount"
        ></el-pagination>

        <el-dialog
            v-if="dialogVisible"
            @close="dialogVisible=false"
            :size="componentSize"
            :title="formTitle"
            :visible.sync="dialogVisible"
            :close-on-click-modal="false"
            :close-on-press-escape="false"
        >
            <slot name="form"></slot>
            <span slot="footer" class="dialog-footer">
                <slot name="dialogBtn"></slot>
            </span>
        </el-dialog>
        <slot name="other"></slot>
    </el-main>
</template>

<script>
import AddEditDeleteBar from '@/components/common/AddEditDeleteBar';
import SearchBar from '@/components/common/SearchBar';
import { DataGridBaseMixin } from '@/common/data-grid-base.js';
export default {
    name: 'BasicManager',
    props: ['searchFnName', 'delFnName', 'showIndexColumn', 'btnAuth'],
    components: {
        AddEditDeleteBar,
        SearchBar
    },
    mixins: [DataGridBaseMixin],
    data() {
        // 读取通用配置
        const config = this.$tools.config;

        return {
            componentSize: config.ComponentSize(),
            SearchFnName: this.searchFnName,
            DelFnName: this.delFnName,
            SearchForm: {},
            HighlightCurrentRow: true,
            // this.$refs.BasicManager.elTableBmStyle = '你的自定义表格样式';
            elTableBmStyle: 'width: 100%'
        };
    },
    methods: {
        LoadGridData(_gridPar, _searchPar) {
            this.OpenLoading();
            this.$client[this.SearchFnName](
                this.$tools.MakeDataGridSearchPart(_gridPar, _searchPar)
            )
                .then(ret => {
                    this.CloseLoading();
                    if (ret) {
                        this.grid = ret.data || {
                            items: [],
                            totalItemCount: 0
                        };
                    } else {
                        this.$message.error(
                            '数据加载失败' + this.SearchFnName || ''
                        );
                    }
                })
                .catch(err => {
                    this.CloseLoading();
                    this.ClientRequestError(this.SearchFnName);
                });
        },
        LoadFormData(type) {
            let data = null;
            if (type) {
                data = this.currentRow;
            }
            this.$emit('loadFormData', data);
        },
        editRow() {
            this.Edit();
        },
        PerformDeleteRow() {
            this.OpenLoading('删除操作中，请稍等...');
            this.$client[this.DelFnName]({
                id: this.currentRow.Id
            })
                .then(ret => {
                    this.CloseLoading();
                    if (ret && ret.code) {
                        this.DelCallBack();
                    } else {
                        this.$message.error(
                            ret.msg || this.$t('msg.delete.fail')
                        );
                    }
                })
                .catch(err => {
                    this.CloseLoading();
                    this.ClientRequestError(this.DelFnName);
                });
        },
        DelCallBack() {
            this.$message({
                message: this.$t('msg.delete.success'),
                type: 'success'
            });
            this.Search(true);
        },
        search(data) {
            this.SearchForm = data;
        },
        searchList() {
            this.Search(true);
        },
        makeSearchPar() {
            return this.SearchForm;
        }
    }
};
</script>

<style scoped>
.table {
    width: 100%;
}

.el-row {
    margin-bottom: 20px;

    &:last-child {
        margin-bottom: 0;
    }
}
</style>
