<template>
    <div
        class="ui-layout-flex search-form-container"
        v-bind:class="[show ? 'xx' : 'search-form-container--nowrap']"
    >
        <el-form
            v-bind:class="[searchBarExpand ? 'search-form search-form--expand' : 'search-form']"
            :model="params"
            :inline="inline"
            ref="form"
            :size="size"
            @submit.native.prevent="searchHandler()"
            label-position="top"
            :label-width="labelWidth ? (labelWidth + 'px') : ''"
        >
            <el-form-item
                v-for="(form, index) in forms"
                :key="index"
                v-show="!form.hidden"
                :prop="form.itemType != 'daterange' ? form.prop : (datePrefix + index)"
                :label="form.label"
                :rules="form.rules || []"
                :label-width="form.labelWidth ? (form.labelWidth + 'px') : ''"
            >
                <el-input
                    v-if="form.itemType === 'input' || form.itemType === undefined"
                    v-bind="form"
                    v-model="params[form.modelValue || form.prop]"
                    :size="form.size ? form.size : size"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                />
                <el-select
                    v-else-if="form.itemType === 'select'"
                    v-bind="form"
                    v-on="form"
                    v-model="params[form.modelValue || form.prop]"
                    :size="form.size ? form.size : size"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                >
                    <el-option
                        v-for="(option, optionIndex) in form.options"
                        :key="optionIndex + '_local'"
                        :value="(typeof option === 'object') ? option[form.valueKey || 'key'] : option"
                        :disabled="(typeof option === 'object') ? option[form.disabledKey || 'disabled'] : false"
                        :label="(typeof option === 'object') ? option[form.labelKey || 'value'] : option"
                    />
                    <el-option
                        v-for="(op, opIndex) in selectOptions[selectOptionPrefix + index]"
                        :key="opIndex + '_remote'"
                        :value="(typeof op === 'object') ? op[form.valueKey || 'key'] : op"
                        :disabled="(typeof op === 'object') ? op[form.disabledKey || 'disabled'] : false"
                        :label="(typeof op === 'object') ? op[form.labelKey || 'value'] : op"
                    />
                </el-select>
                <el-select
                    v-else-if="form.itemType === 'selectOrg'"
                    v-bind="form"
                    v-on="form"
                    v-model="params[form.modelValue || form.prop]"
                    :size="form.size ? form.size : size"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                >
                    <el-option
                        v-for="(option, optionIndex) in form.options"
                        :key="optionIndex + '_local'"
                        :value="(typeof option === 'object') ? option[form.valueKey || 'key'] : option"
                        :disabled="(typeof option === 'object') ? option[form.disabledKey || 'disabled'] : false"
                        :label="(typeof option === 'object') ? option[form.labelKey || 'value'] : option"
                    >
                        <div>
                            <span
                                style="float: left;"
                                :class="{'is-delete': option.isDeleted, 'is-forbidde': option.state === 'N'}"
                            >{{option.name}}</span>
                            <span
                                style="margin-left: 15px;float: right;color: #8492a6;font-size: 13px;margin-right: 12px;"
                            >{{option.orgs.join('、')}}</span>
                        </div>
                    </el-option>
                </el-select>
                <el-date-picker
                    v-else-if="form.itemType === 'date'"
                    v-model="params[form.modelValue || form.prop]"
                    type="date"
                    v-bind="form"
                    v-on="form"
                    :size="form.size ? form.size : size"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                    :picker-options="form.pickerOptions || {}"
                />
                <el-date-picker
                    v-else-if="form.itemType === 'datetime'"
                    v-model="params[form.modelValue || form.prop]"
                    type="datetime"
                    v-bind="form"
                    v-on="form"
                    :format="form.format"
                    :size="form.size ? form.size : size"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                    :picker-options="form.pickerOptions || {}"
                />
                <el-date-picker
                    v-else-if="form.itemType === 'daterange'"
                    v-model="params[form.modelValue || form.prop]"
                    :size="form.size ? form.size : size"
                    v-bind="form"
                    v-on="form"
                    type="daterange"
                    @change="date => changeDate(date, form.prop[0], form.prop[1])"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                    :picker-options="form.pickerOptions || {}"
                />
                <el-checkbox
                    v-else-if="form.itemType === 'checkbox'"
                    v-modal="params[form.modelValue || form.prop]"
                    :size="form.size?form.size:size"
                    v-bind="form"
                    v-on="form"
                    :style="itemStyle + (form.itemWidth ? `width: ${form.itemWidth}px;` : '')"
                />
            </el-form-item>
        </el-form>
        <div class="ui-layout-flex--items_center form-actions">
            <el-button
                type="primary"
                :size="size"
                round
                class="ex-button ex-button--submit"
                @click="searchHandler"
                :loading="submitLoading"
            >{{ $t(submitBtnText) }}</el-button>
            <el-button
                type="primary"
                :size="size"
                round
                v-if="showResetBtn"
                class="ex-button ex-button--reset"
                @click="resetForm"
                :loading="submitLoading"
            >{{ $t(resetBtnText) }}</el-button>
            <el-button
                :style="{visibility: formExpandBtnIsShow ? 'visible' : 'hidden'}"
                :size="size"
                class="expand-btn"
                icon="el-icon-d-arrow-right search-plan-btn"
                @click="advancedSearch"
                round
            >{{$t('common.more')}}</el-button>
        </div>
    </div>
</template>

<script>
import {formProps} from './props';
export default {
    name: 'ExSearchForm',
    props: formProps,
    data() {
        const {forms, fuzzy} = this.$props;
        const datePrefix = 'daterange-prefix';
        const selectOptionPrefix = 'select-option-prefix';
        const dataObj = {
            selectOptions: {}
        };
        const params = {};
        const formatter = {};
        const fuzzyOps = {};
        forms.forEach((v, i) => {
            const propType = typeof v.prop;
            if (propType === 'string') {
                v.modelValue = v.prop;
                params[v.prop] = v.defaultValue || '';
                fuzzyOps[v.prop] = v.fuzzy ? v.fuzzy : fuzzy;
                if (v.formatter) {
                    formatter[v.prop] = v.formatter;
                }
            } else if (propType === 'object' && Object.prototype.toString.call(v.prop) === '[object Array]') {
                v.prop.forEach(vv => {
                    params[vv] = '';
                    if (v.formatter) {
                        formatter[vv] = v.formatter;
                    }
                    fuzzyOps[vv] = v.fuzzy ? v.fuzzy : fuzzy;
                });
            }
            if (v.itemType === 'daterange') {
                params[datePrefix + i] = v.defaultValue || '';
                v.modelValue = datePrefix + i;
            }
            if (v.itemType === 'select' && (v.selectFetch || v.selectUrl)) {
                const dataKey = selectOptionPrefix + i;
                dataObj.selectOptions[dataKey] = [];
                const {$axios} = this;
                if (!v.selectMethod) {
                    v.selectMethod = 'get';
                }
                const remoteFetch = () => {
                    return $axios[v.selectMethod](
                        v.selectUrl,
                        v.selectMethod.toLowerCase() === 'get' ? {params: v.selectParams} : v.selectParams
                    );
                };
                this.getRemoteData({
                    fetch: v.selectFetch ? v.selectFetch : remoteFetch,
                    dataKey,
                    resultField: v.selectResultField || 'result',
                    resultHandler: v.selectResultHandler
                });
            }
        });
        return {
            params,
            datePrefix,
            selectOptionPrefix,
            ...dataObj,
            formatter,
            fuzzyOps,
            formExpandBtnIsShow: false,
            searchBarExpand: false,
            show: false
        };
    },
    computed: {
        itemStyle() {
            const {itemWidth} = this;
            if (itemWidth) {
                return `width: ${itemWidth}px;`;
            }
            return '';
        }
    },
    watch: {
        params: {
            handler(val) {
                if (typeof this.formValueChange === 'function') {
                    this.formValueChange(val);
                }
            },
            deep: true,
            immediate: true
        },
        //to fix Bug #46028
        forms: {
            handler() {
                this.$nextTick(() => {
                    this.adaptiveSearchBar();
                });
            }
        }
    },
    methods: {
        advancedSearch() {
            this.searchBarExpand = !this.searchBarExpand;
        },

        adaptiveSearchBar() {
            //搜索区域宽度
            const containerNode = document.querySelector('.search-form-container');
            const containerWidth = containerNode ? containerNode.getBoundingClientRect().width : 0;
            //搜索表单宽度(不换行直列的情况下)
            const searchFormNode = document.querySelector('.search-form');
            const searchFormWidth = searchFormNode ? searchFormNode.getBoundingClientRect().width : 0;
            //表单按钮区域宽度
            const formActionNode = document.querySelector('.form-actions');
            const formActionWidth = formActionNode ? formActionNode.getBoundingClientRect().width : 0;

            //当搜索区域所有表单项一行排列的宽+按钮宽 < 总容器的宽度, 则换行表单元素
            if (searchFormWidth + formActionWidth < containerWidth) {
                this.formExpandBtnIsShow = false;
            } else {
                this.formExpandBtnIsShow = true;
            }
            this.searchBarExpand = false;
            this.$nextTick(() => {
                this.show = true;
            });
        },
        isArray(value) {
            return typeof value === 'object' && Object.prototype.toString.call(value) === '[object Array]';
        },
        searchHandler() {
            this.getParams((error, params) => {
                if (!error) {
                    const {submitHandler} = this;
                    if (submitHandler) {
                        submitHandler(params);
                    } else {
                        throw new Error('Need to set attribute: submitHandler !');
                    }
                }
            });
        },
        getParamFuzzy() {
            return this.fuzzyOps;
        },
        getParams(callback) {
            this.$refs.form.validate(valid => {
                if (valid) {
                    const {params, datePrefix, formatter} = this;
                    const formattedForm = {};
                    Object.keys(params).forEach(v => {
                        if (v.indexOf(datePrefix) === -1) {
                            formattedForm[v] = formatter[v] ? formatter[v](params[v], v) : params[v];
                        }
                    });
                    if (callback) {
                        callback(null, formattedForm);
                    }
                } else if (callback) {
                    callback(new Error());
                }
            });
        },
        resetForm() {
            this.$refs.form.resetFields();
            this.searchHandler();
            const {resetBtnCallback} = this;
            if (resetBtnCallback) {
                resetBtnCallback();
            }
        },
        changeDate(date, startDate, endDate) {
            let dates;
            if (date === null) {
                this.params[startDate] = '';
                this.params[endDate] = '';
                return;
            }
            if (typeof date === 'string') {
                dates = date.split(' - ');
            } else if (date && date.hasOwnProperty('length')) {
                const firstDate = date[0];
                const secondDate = date[1];
                dates = [
                    // eslint-disable-next-line
                    `${firstDate.getFullYear()}-${('0' + (firstDate.getMonth() + 1)).substr(-2)}-${(
                        '0' + firstDate.getDate()
                    )
                        // eslint-disable-next-line
                        .substr(-2)}`,
                    // eslint-disable-next-line
                    `${secondDate.getFullYear()}-${('0' + (secondDate.getMonth() + 1)).substr(-2)}-${(
                        '0' + secondDate.getDate()
                    )
                        // eslint-disable-next-line
                        .substr(-2)}`
                ];
            } else {
                this.params[startDate] = '';
                this.params[endDate] = '';
                return;
            }
            this.params[startDate] = dates[0];
            this.params[endDate] = dates[1];
        },
        getRemoteData({fetch, dataKey, resultField}) {
            fetch().then(response => {
                let result = response;
                if (typeof response === 'object' && !this.isArray(response)) {
                    if (resultField.indexOf('.') !== -1) {
                        resultField.split('.').forEach(vv => {
                            result = result[vv];
                        });
                    } else {
                        result = response[resultField];
                    }
                }
                if (!result || !(result instanceof Array)) {
                    // eslint-disable-next-line
                    console.warn(
                        `The result of key:${resultField} is not Array. 接口返回的字段:${resultField} 不是一个数组`
                    );
                }
                if (this.resultHandler) {
                    this.selectOptions[dataKey] = result.map(this.resultHandler);
                } else {
                    this.selectOptions[dataKey] = result;
                }
            });
        }
    },
    mounted() {
        this.adaptiveSearchBar();
        window.onresize = () => {
            if (this.timer) {
                clearTimeout(this.timer);
                this.timer = 0;
            }
            this.timer = setTimeout(() => {
                this.adaptiveSearchBar();
            }, 200);
        };
    },
    beforeDestroy() {
        window.onresize = null;
    }
};
</script>

<style lang="less" scoped>
.search-form-container {
    flex-shrink: 0;
}
.form-actions {
    flex-shrink: 0;
    margin-top: 26px;
}
.search-form--expand {
    height: auto !important;
}
.search-form-container--nowrap {
    visibility: hidden;
    & > .search-form {
        white-space: nowrap;
    }
}
.search-form {
    height: 73px;
    overflow: hidden;
}
.is-delete {
    text-decoration: line-through;
}

.is-forbidden {
    color: red;
}
</style>
