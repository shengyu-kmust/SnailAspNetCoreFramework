    <template>
    <div class="ui-height-100 ui-layout-flex ui-layout-flex--column">
        <ex-search-form
            v-if="formOptions"
            ref="searchForm"
            v-bind="formOptions"
            :submit-handler="searchHandler"
            :submit-loading="loading"
        />
        <slot name="form" :loading="loading" :search="searchHandler" />

        <div class="ui-layout-flex--1">
            <el-table
                v-loading.lock="loading"
                ref="table"
                :data="tableData"
                :border="border"
                :size="size"
                :stripe="stripe"
                :height="height"
                :max-height="maxHeight"
                :fit="fit"
                :show-overflow-tooltip="showOverflowTooltip"
                :show-header="showHeader"
                :highlight-current-row="highlightCurrentRow"
                :current-row-key="currentRowKey"
                :row-class-name="rowClassName"
                :row-style="rowStyle"
                :row-ket="rowKey"
                :empty-text="emptyText"
                :default-expand-all="defaultExpandAll"
                :expand-row-keys="expandRowKeys"
                :default-sort="defaultSort"
                :tooltip-effect="tooltipEffect"
                :show-summary="showSummary"
                :sum-text="sumText"
                :summary-method="summaryMethod"
                @select="(selection, row) => emitEventHandler('select', selection, row)"
                @select-all="selection => emitEventHandler('select-all', selection)"
                @selection-change="selection => emitEventHandler('selection-change', selection)"
                @cell-mouse-enter="(row, column, cell, event) => emitEventHandler('cell-mouse-enter', row, column, cell, event)"
                @cell-mouse-leave="(row, column, cell, event) => emitEventHandler('cell-mouse-leave', row, column, cell, event)"
                @cell-click="(row, column, cell, event) => emitEventHandler('cell-click', row, column, cell, event)"
                @cell-dblclick="(row, column, cell, event) => emitEventHandler('cell-dblclick', row, column, cell, event)"
                @row-click="(row, event, column) => emitEventHandler('row-click', row, event, column)"
                @row-dblclick="(row, event) => emitEventHandler('row-dblclick', row, event)"
                @row-contextmenu="(row, event) => emitEventHandler('row-contextmenu', row, event)"
                @header-click="(column, event) => emitEventHandler('header-click', column, event)"
                @sort-change="args => emitEventHandler('sort-change', args)"
                @filter-change="filters => emitEventHandler('filter-change', filters)"
                @current-change="(currentRow, oldCurrentRow) => emitEventHandler('current-change', currentRow, oldCurrentRow)"
                @header-dragend="(newWidth, oldWidth, column, event) => emitEventHandler('header-dragend', newWidth, oldWidth, column, event)"
                @expand-change="(row, expanded) => emitEventHandler('expand-change', row, expanded)"
            >
                <slot name="prepend" />
                <el-table-column type="index" width="50">
                    <template slot="header">{{$t("common.serialNumber")}}</template>
                </el-table-column>
                <template v-for="(column, columnIndex) in columns">
                    <el-table-column
                        :key="columnIndex"
                        :column-key="column.columnKey"
                        :prop="column.prop"
                        :label="column.label"
                        :width="column.minWidth ? '-' : (column.width || 'auto')"
                        :minWidth="column.minWidth || column.width || 'auto'"
                        :fixed="column.fixed"
                        :render-header="column.renderHeader"
                        :sortable="column.sortable"
                        :sort-by="column.sortBy"
                        :sort-method="column.method"
                        :resizable="column.resizable"
                        :formatter="column.formatter"
                        :show-overflow-tooltip="column.showOverflowTooltip == null ?  showOverflowTooltip : column.showOverflowTooltip "
                        :align="column.align || 'center'"
                        :header-align="column.headerAlign || column.align"
                        :class-name="column.className"
                        :label-class-name="column.labelClassName"
                        :selectable="column.selectable"
                        :reserve-selection="column.reserveSelection"
                        :filters="column.filters"
                        :filter-placement="column.filterPlacement"
                        :filter-multiple="column.filterMultiple"
                        :filter-method="column.filterMethod"
                        :filtered-value="column.filteredValue"
                        v-if="column.type === undefined"
                    >
                        <template slot-scope="scope" :scope="newSlotScope ? 'scope' : false">
                            <span
                                v-if="column.filter"
                            >{{ Vue.filter(column['filter'])(scope.row[column.prop]) }}</span>
                            <span v-else-if="column.slotName">
                                <slot
                                    :name="column.slotName"
                                    :row="scope.row"
                                    :$index="scope.$index"
                                />
                            </span>
                            <span v-else-if="column.render">{{ column.render(scope.row) }}</span>
                            <span
                                v-else-if="column.renderHtml"
                                v-html="column.renderHtml(scope.row)"
                            ></span>
                            <span
                                v-else-if="column.formatter"
                            >{{ column.formatter(scope.row, scope.column, scope.row[column.prop], scope.$index) }}</span>
                            <span v-else>{{ scope.row[column.prop] }}</span>
                        </template>
                    </el-table-column>
                    <el-table-column v-bind="column" :key="columnIndex" v-else></el-table-column>
                </template>
                <slot name="append" />
            </el-table>
        </div>
        <div v-if="showPagination" style="margin-top: 10px;text-align: right;">
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="pagination.pageIndex"
                :page-sizes="pageSizes"
                :page-size="pagination.pageSize"
                :layout="paginationLayout"
                :total="total"
            ></el-pagination>
        </div>
    </div>
</template>

<script>
import props from './props';
import ExSearchForm from '../ExSearchForm/ExSearchForm';
export default {
    name: 'ExSearchTablePagination',
    components: {
        ExSearchForm
    },
    props,
    data() {
        return {
            pagination: {
                pageIndex: 1,
                pageSize: (() => {
                    const {pageSizes} = this;
                    if (pageSizes.length > 0) {
                        return pageSizes[0];
                    }
                    // eslint-disable-next-line
                    return 20;
                })(this)
            },
            total: 0,
            loading: false,
            tableData: [],
            cacheLocalData: []
        };
    },
    computed: {},
    methods: {
        search() {
            this.$refs.searchForm.searchHandler();
        },
        handleSizeChange(size) {
            this.pagination.pageSize = size;
            this.dataChangeHandler();
        },
        handleCurrentChange(pageIndex) {
            this.$nextTick(() => {
                this.$refs.table.bodyWrapper.scrollTop = 0;
            });
            this.pagination.pageIndex = pageIndex;
            this.dataChangeHandler();
        },
        searchHandler(resetPageIndex = true) {
            if (resetPageIndex) {
                this.pagination.pageIndex = 1;
            }
            this.dataChangeHandler(arguments[0]);
        },
        dataChangeHandler() {
            const {type} = this;
            if (type === 'local') {
                this.dataFilterHandler(arguments[0]);
            } else if (type === 'remote') {
                this.fetchHandler(arguments[0]);
            }
        },
        dataFilter(data) {
            const {pageIndex, pageSize} = this.pagination;
            return data.filter((v, i) => {
                return i >= (pageIndex - 1) * pageSize && i < pageIndex * pageSize;
            });
        },
        dataFilterHandler(formParams) {
            const {cacheLocalData, params} = this;
            const mergeParams = Object.assign(params, formParams);
            const validParamKeys = Object.keys(mergeParams).filter(v => {
                return mergeParams[v] !== undefined && mergeParams[v] !== '';
            });
            const searchForm = this.$refs.searchForm;
            let paramFuzzy;
            if (searchForm) {
                paramFuzzy = searchForm.getParamFuzzy();
            }
            if (validParamKeys.length > 0) {
                const validData = cacheLocalData.filter(v => {
                    const valids = [];
                    validParamKeys.forEach(vv => {
                        if (typeof v[vv] === 'number') {
                            valids.push(
                                paramFuzzy && paramFuzzy[vv]
                                    ? String(v[vv]).indexOf(String(mergeParams[vv])) !== -1
                                    : String(v[vv]) === String(mergeParams[vv])
                            );
                        } else {
                            valids.push(
                                paramFuzzy && paramFuzzy[vv]
                                    ? v[vv].indexOf(mergeParams[vv]) !== -1
                                    : v[vv] === mergeParams[vv]
                            );
                        }
                    });
                    return valids.every(vvv => {
                        return vvv;
                    });
                });
                this.tableData = this.dataFilter(validData);
                this.total = validData.length;
            } else {
                this.total = cacheLocalData.length;
                this.tableData = this.dataFilter(cacheLocalData);
            }
        },

        deepMatch(keys, ret) {
            let data = ret;
            if (keys && keys.indexOf('.') !== -1) {
                const keyArr = keys.split('.');
                for (let i = 0, len = keyArr.length; i < len; i++) {
                    data = data[keyArr[i]];
                    if (data == null) {
                        break;
                    }
                }
            } else {
                data = data[keys];
            }
            return data;
        },
        fetchHandler(formParams = {}) {
            const {
                paramsMethod,
                fetch,
                listField,
                pageIndexKey,
                pageSizeKey,
                totalField,
                showPagination,
                pagination
            } = this;
            let {params} = this;
            params = JSON.parse(JSON.stringify(Object.assign(params, formParams)));
            if (showPagination) {
                params = Object.assign(params, {
                    [pageIndexKey]: pagination.pageIndex,
                    [pageSizeKey]: pagination.pageSize
                });
            }
            if (Object.prototype.toString.call(paramsMethod) === '[object Function]') {
                paramsMethod(params);
            }
            if (Object.prototype.toString.call(fetch) === '[object Function]') {
                fetch(params)
                    .then(res => {
                        console.log('fetchHandler');
                        let ret;
                        if (typeof this.fetchHandle === 'function') {
                            ret = this.fetchHandle(res);
                        } else {
                            ret = res;
                        }
                        const rows = this.deepMatch(listField, ret);
                        //如果匹配出来的rows不是一个数组，则抛出异常
                        if (Object.prototype.toString.call(rows) !== '[object Array]') {
                            throw new Error(`The result of key:${listField} is not Array.`);
                        }
                        //自定义处理数据
                        if (Object.prototype.toString.call(this.dataHandler) === '[object Function]') {
                            this.tableData = rows.map(this.dataHandler);
                        } else {
                            this.tableData = rows;
                        }
                        //根据props取对应的total
                        this.total = this.deepMatch(totalField, ret) || 0;
                    })
                    // eslint-disable-next-line
                    .finally(() => {});
            }
        },
        emitEventHandler(event, row, column) {
            if (event === 'row-click') {
                if (column.type !== 'action') {
                    const currentRow = Array.from(arguments)[1];
                    if (currentRow) {
                        this.$refs.table.toggleRowSelection(currentRow);
                    }
                }
            }
            this.$emit(event, ...Array.from(arguments).slice(1));
        },

        loadLocalData(data) {
            const {autoLoad} = this;
            if (!data) {
                this.showPagination = false;
                throw new Error("When the type is 'local', you must set attribute 'data' and 'data' must be a array.");
            }
            const cacheData = JSON.parse(JSON.stringify(data));
            this.cacheLocalData = cacheData;
            if (autoLoad) {
                this.tableData = this.dataFilter(cacheData);
                this.total = cacheData.length;
            }
        }
    },
    mounted() {
        // event: expand changed to `expand-change` in Element v2.x
        this.$refs.table.$on('expand', (row, expanded) => this.emitEventHandler('expand', row, expanded));
        const {type, autoLoad, data, formOptions, params} = this;
        if (type === 'remote' && autoLoad) {
            if (formOptions) {
                this.$refs.searchForm.getParams((error, formParams) => {
                    if (!error) {
                        this.fetchHandler(Object.assign(formParams, params));
                    }
                });
            } else {
                this.fetchHandler(params);
            }
        } else if (type === 'local') {
            this.loadLocalData(data);
        }
    },
    watch: {
        data(value) {
            this.loadLocalData(value);
        }
    }
};
</script>
